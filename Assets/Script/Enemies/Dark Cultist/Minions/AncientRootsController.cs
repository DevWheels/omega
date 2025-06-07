using UnityEngine;
using Mirror;
using System.Collections;

public class AncientRootsController : MinionController
{
    [Header("Настройки Древних Корней")]
    [SerializeField] private float _rootDuration = 3f; // Длительность обездвиживания
    [SerializeField] private float _rootCooldown = 8f; // Время между атаками
    [SerializeField] private float _rootRange = 2.5f; // Дистанция атаки
    [SerializeField] private GameObject _rootEffectPrefab; // Эффект опутывания
    [SerializeField] private float _fireDamageMultiplier = 2f; // Множитель урона от огня
    
    private float _lastRootTime;
    private bool _isRooting = false;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed *= 0.3f; 
        fireResistance = 0.5f;
    }

    [ServerCallback]
    protected override void Update()
    {
        base.Update();

        // Проверяем возможность атаки
        if (!_isRooting && 
            Time.time > _lastRootTime + _rootCooldown && 
            _target != null && 
            Vector2.Distance(transform.position, _target.position) <= _rootRange)
        {
            TryRootPlayer();
        }
    }

    [Server]
    private void TryRootPlayer()
    {
        // Проверяем по тегу, что цель является игроком
        if (_target == null || !_target.CompareTag("Player"))
        {
            _isRooting = false;
            return;
        }

        _lastRootTime = Time.time;
        _isRooting = true;

        // Визуальные эффекты
        RpcPlayRootEffect(true);
    
        PlayerStats playerStats = _target.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            NetworkIdentity targetIdentity = playerStats.GetComponent<NetworkIdentity>();
            if (targetIdentity != null && targetIdentity.connectionToClient != null)
            {
                TargetRootPlayer(targetIdentity.connectionToClient);
            }
            else
            {
                Debug.LogWarning("Не удалось получить connectionToClient у цели");
                _isRooting = false;
                return;
            }
        }
        else
        {
            Debug.LogWarning("PlayerStats не найден у игрока");
            _isRooting = false;
            return;
        }

        StartCoroutine(EndRootAfterDelay(_rootDuration));
    }

    [TargetRpc]
    private void TargetRootPlayer(NetworkConnection target)
    {
        if (target == null || target.identity == null) return;
    
        PlayerMovement movement = target.identity.GetComponent<PlayerMovement>();
        if (movement != null && movement.isLocalPlayer)
        {
            movement.ApplyRoot(_rootDuration);
        }
    }
    [Server]
    private IEnumerator EndRootAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isRooting = false;
        RpcPlayRootEffect(false);
    }

    [ClientRpc]
    private void RpcPlayRootEffect(bool start)
    {
        if (_rootEffectPrefab != null)
        {
            // Здесь должна быть логика активации/деактивации эффектов
            if (start)
            {
                Instantiate(_rootEffectPrefab, _target.position, Quaternion.identity);
            }
        }
    }


    [Server]
    public override void TakeDamage(int damage, DamageType damageType = DamageType.Physical)
    {
        // Уязвимость к огню
        if (damageType == DamageType.Fire)
        {
            damage = Mathf.RoundToInt(damage * 1.5f); // +50% урона
            Debug.Log($"Ancient Roots take extra fire damage: {damage}");
        }

        base.TakeDamage(damage, damageType);
    }

    protected override void HandleMovement()
    {
        // Корни медленно тянутся к цели
        if (_target != null && !_isRooting)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }

    protected override void Attack()
    {
        // Атака реализована через TryRootPlayer
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _rootRange);
    }
}