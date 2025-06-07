using UnityEngine;
using Mirror;
using System.Collections;

public class EtherealMantleController : MinionController
{
    [Header("Настройки Эфирной Мантии")]
    [SerializeField] private float _teleportCooldown = 5f; // Время между телепортами
    [SerializeField] private float _teleportDistance = 1f; // Уменьшенная дистанция телепортации (рядом с игроком)
    [SerializeField] private float _vanishDuration = 1f; // Длительность исчезновения
    [SerializeField] private float _flightHeight = 1.5f; // Высота полета
    [SerializeField] private GameObject _teleportEffectPrefab;
    [SerializeField] private GameObject _vanishEffectPrefab;
    [SerializeField] private float _minTeleportAngle = 120f; // Минимальный угол для телепортации
    [SerializeField] private float _attackRange = 1.5f; // Дистанция атаки (независимая от коллайдера)
    
    private float _lastTeleportTime;
    private bool _isVanished = false;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    protected override void Awake()
    {
        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        lightningResistance = 1.5f;
        fireResistance = 0.7f;
        // Начальная позиция в воздухе
        transform.position += Vector3.up * _flightHeight;
    }

    [ServerCallback]
    protected override void Update()
    {
        if (_isVanished) return;
        
        base.Update();
        
        // Проверяем возможность телепортации
        if (Time.time > _lastTeleportTime + _teleportCooldown && 
            _target != null && 
            IsBehindPlayer())
        {
            TryTeleportBehindPlayer();
        }
        
        // Проверка дистанции для атаки (независимая от коллайдера)
        if (_target != null && Vector2.Distance(transform.position, _target.position) <= _attackRange)
        {
            Attack();
        }
    }

    [Server]
    private bool IsBehindPlayer()
    {
        if (_target == null) return false;
    
        Vector2 toPlayer = (Vector2)(_target.position - transform.position);
        Vector2 playerForward = _target.right; // Предполагаем, что игрок смотрит по правой оси
    
        float angle = Vector2.Angle(toPlayer, playerForward);
        return angle >= _minTeleportAngle;
    }

    [Server]
    private void TryTeleportBehindPlayer()
    {
        if (_target == null) return;
    
        _lastTeleportTime = Time.time;
    
        // Телепортация ближе к игроку (используем уменьшенную _teleportDistance)
        Vector2 teleportPosition = (Vector2)_target.position - (Vector2)_target.right * _teleportDistance;
        teleportPosition += Vector2.up * _flightHeight; // Сохраняем высоту полета
    
        RpcPlayTeleportEffect(false); // Эффект исчезновения
        SetVanishState(true);
    
        StartCoroutine(TeleportAfterDelay(0.3f, teleportPosition));
    }

    [Server]
    private IEnumerator TeleportAfterDelay(float delay, Vector2 position)
    {
        yield return new WaitForSeconds(delay);
        
        transform.position = position;
        RpcPlayTeleportEffect(true); // Эффект появления
        SetVanishState(false);
    }

    [Server]
    private void SetVanishState(bool vanished)
    {
        _isVanished = vanished;
        RpcSetVisualState(vanished);
        _collider.enabled = !vanished; // Коллайдер отключается только при исчезновении
    }

    protected override void HandleMovement()
    {
        if (_target == null || _isVanished) return;
        
        // Плавное парение вместо обычного движения
        Vector2 direction = (_target.position - transform.position).normalized;
        direction.y = 0; // Сохраняем высоту полета
        
        _rb.linearVelocity = direction * _moveSpeed;
        
        // Поддерживаем высоту полета
        float currentHeight = transform.position.y;
        float targetHeight = _target.position.y + _flightHeight;
        float heightDifference = targetHeight - currentHeight;
        
        _rb.linearVelocity += Vector2.up * heightDifference;
    }

    protected override void Attack()
    {
        if (!_health.CanAttack() || _isVanished) return;
        
        _health.ResetAttackCooldown();
        _target.GetComponent<PlayerStats>().TakeHit(_health.CurrentAttack);
        
        // Исчезаем после атаки
        StartCoroutine(VanishAfterAttack());
    }

    [Server]
    private IEnumerator VanishAfterAttack()
    {
        SetVanishState(true);
        RpcPlayVanishEffect();
        
        yield return new WaitForSeconds(_vanishDuration);
        
        SetVanishState(false);
    }

    [ClientRpc]
    private void RpcPlayTeleportEffect(bool isAppearing)
    {
        if (_teleportEffectPrefab != null)
        {
            GameObject effect = Instantiate(_teleportEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.localScale = isAppearing ? Vector3.one : Vector3.one * 0.8f;
            Destroy(effect, 1f);
        }
    }

    [ClientRpc]
    private void RpcPlayVanishEffect()
    {
        if (_vanishEffectPrefab != null)
        {
            GameObject effect = Instantiate(_vanishEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    [ClientRpc]
    private void RpcSetVisualState(bool vanished)
    {
        _spriteRenderer.enabled = !vanished;
    }
}