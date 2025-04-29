using Mirror;
using UnityEngine;
public class Projectile : NetworkBehaviour
{
    [SyncVar] private Transform target;
    [SyncVar] private int damage; 
    [SyncVar] private float speed;

    [Server]
    public void Initialize(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerStats>(out var health))
        {
            health.TakeHit(damage); 
            NetworkServer.Destroy(gameObject);
        }
    }
}