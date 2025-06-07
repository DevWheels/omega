// CloneBehavior.cs
using Mirror;
using UnityEngine;

public class CloneBehavior : NetworkBehaviour
{
    [SerializeField] private int cloneDamage = 1;
    [SerializeField] private float cloneLifetime = 20f;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float scatterSpeedMultiplier = 2f;
    
    private Transform originalEnemy;
    private TricksterEnemyAI tricksterAI;
    private float spawnTime;
    private bool isScattering = false;
    private Vector2 scatterDirection;

    public void Initialize(Transform original, TricksterEnemyAI ai)
    {
        originalEnemy = original;
        tricksterAI = ai;
        spawnTime = Time.time;
    }

    public void ScatterAway(Vector3 fromPosition)
    {
        isScattering = true;
        scatterDirection = ((Vector2)transform.position - (Vector2)fromPosition).normalized;
    }

    private void Update()
    {
        if (!isServer) return;

        if (Time.time > spawnTime + cloneLifetime)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }

        if (isScattering)
        {
            // Разбегаемся
            transform.position += (Vector3)(scatterDirection * moveSpeed * scatterSpeedMultiplier * Time.deltaTime);
        }
        else if (originalEnemy != null)
        {
            // Двигаемся вокруг оригинального врага
            if (Vector2.Distance(transform.position, originalEnemy.position) > followDistance)
            {
                Vector2 direction = ((Vector2)originalEnemy.position - (Vector2)transform.position).normalized;
                transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Крутимся вокруг врага
                transform.position += transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeHit(cloneDamage);
            }
        }
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            tricksterAI?.CloneDestroyed(gameObject);
            NetworkServer.Destroy(gameObject);
        }
    }
}