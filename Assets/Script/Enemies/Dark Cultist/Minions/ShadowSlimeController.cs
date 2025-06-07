using UnityEngine;
using Mirror;
using System.Collections;

public class ShadowSlimeController : MinionController
{
    [Header("Настройки Теневого Слизня")]
    [SerializeField] private float _slowAmount = 0.5f; // Множитель замедления
    [SerializeField] private float _poisonDamage = 3f; // Урон от яда
    [SerializeField] private float _poisonInterval = 1f; // Интервал урона
    [SerializeField] private float _poisonDuration = 5f; // Длительность эффекта
    [SerializeField] private GameObject _poisonPoolPrefab; // Префаб ядовитой лужи
    [SerializeField] private float _poolSpawnInterval = 2f; // Интервал оставления луж
    [SerializeField] private int _maxPools = 3; // Максимум луж за жизнь
    [SerializeField] private float _poolLifetime = 8f; // Время жизни лужи

    private float _lastPoolSpawnTime;
    private int _spawnedPoolsCount;
    private float _originalMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        _originalMoveSpeed = _moveSpeed;
        _moveSpeed *= 0.6f; // Слизни медленнее обычных миньонов
        poisonResistance = 0.3f; // 70% сопротивления к яду
        holyResistance = 1.4f; // +40% урона от святого
        fireResistance = 0.8f; // 20% сопротивления к огню
    }

    [ServerCallback]
    protected override void Update()
    {
        base.Update();

        // Оставляем ядовитые лужи с интервалом
        if (Time.time > _lastPoolSpawnTime + _poolSpawnInterval && _spawnedPoolsCount < _maxPools)
        {
            SpawnPoisonPool();
            _lastPoolSpawnTime = Time.time;
            _spawnedPoolsCount++;
        }
    }

    [Server]
    private void SpawnPoisonPool()
    {
        if (_poisonPoolPrefab == null) return;

        GameObject pool = Instantiate(_poisonPoolPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(pool);

        // Уничтожаем лужу через время
        StartCoroutine(DestroyPoolAfterTime(pool, _poolLifetime));
    }

    [Server]
    private IEnumerator DestroyPoolAfterTime(GameObject pool, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (pool != null)
        {
            NetworkServer.Destroy(pool);
        }
    }

    protected override void HandleMovement()
    {
        // Слизни двигаются медленно и плавно
        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
        }
    }

    protected override void Attack()
    {
        // Основной урон через контакт (реализован в базовом классе)
        base.Attack();
    
        // Дополнительно накладываем эффект яда
        if (_health.CanAttack() && _target != null)
        {
            _health.ResetAttackCooldown();
            ApplyPoisonEffect(_target.GetComponent<PlayerStats>());
        }
    }

    [Server]
    private void ApplyPoisonEffect(PlayerStats player)
    {
        if (player == null) return;

        // Отправляем эффект яда игроку
        NetworkIdentity targetIdentity = player.GetComponent<NetworkIdentity>();
        if (targetIdentity != null && targetIdentity.connectionToClient != null)
        {
            TargetApplyPoisonEffect(targetIdentity.connectionToClient);
        }
    }

    [TargetRpc]
    private void TargetApplyPoisonEffect(NetworkConnection target)
    {
        if (target == null || target.identity == null) return;

        PlayerStats player = target.identity.GetComponent<PlayerStats>();
        if (player != null)
        {
            StartCoroutine(PoisonEffectCoroutine(player));
        }
    }

    private IEnumerator PoisonEffectCoroutine(PlayerStats player)
    {
        // Сохраняем оригинальную скорость
        float originalSpeed = player.MovementSpeed;
    
        // Применяем замедление
        player.MovementSpeed *= _slowAmount;
    
        // Визуальный эффект
        player.RpcShowPoisonEffect();

        // Периодический урон (конвертируем в int)
        int poisonDamage = Mathf.RoundToInt(_poisonDamage);
        float endTime = Time.time + _poisonDuration;
        while (Time.time < endTime && player != null)
        {
            player.TakeHit(poisonDamage);
            yield return new WaitForSeconds(_poisonInterval);
        }

        // Восстанавливаем скорость
        if (player != null)
        {
            player.MovementSpeed = originalSpeed;
            player.RpcHidePoisonEffect();
        }
    }

    // Для визуализации в редакторе
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}