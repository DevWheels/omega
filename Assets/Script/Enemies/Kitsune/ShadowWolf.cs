using UnityEngine;
using Mirror;

public class ShadowWolf : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 15f;
    
    private Transform target;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeHit(damage);
            }
            Destroy(gameObject);
        }
    }
}