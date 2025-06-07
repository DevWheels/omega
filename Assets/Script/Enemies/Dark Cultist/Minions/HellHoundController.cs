using System.Collections;
using UnityEngine;
using Mirror;

public class HellHoundController : MinionController
{
    [Header("Настройки Адских Гончих")]
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDamage = 15f;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _runSpeedMultiplier = 1.5f;

    private bool _hasExploded = false;

    protected override void Awake()
    {
        base.Awake();
        _moveSpeed *= _runSpeedMultiplier;
        fireResistance = 0.2f;
        iceResistance = 1.5f;
    }

    protected override void HandleMovement()
    {
        if (_target != null)
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            _rb.linearVelocity = direction * _moveSpeed;
        }
    }

    protected override void Attack()
    {
        // Гончие не имеют обычной атаки
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_hasExploded) return;
        
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            Explode();
        }
    }

    protected override void HandleDeath()
    {
        if (_hasExploded) return;
        Explode();
    }

    [Server]
    private void Explode()
    {
        _hasExploded = true;
    
        RpcPlayExplosionEffect();
    
        int damage = Mathf.RoundToInt(_explosionDamage);
    
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (var hit in hits)
        {
            PlayerStats player = hit.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeHit(damage);
            }
        }
    
        StartCoroutine(DestroyAfterDelay(0.5f));
    }

    [Server]
    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcPlayExplosionEffect()
    {
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}