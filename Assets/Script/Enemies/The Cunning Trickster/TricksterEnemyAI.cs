// TricksterEnemyAI.cs
using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TricksterEnemyAI : NetworkBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float keepDistance = 4f;
    [SerializeField] private float patrolRadius = 10f;
    
    [Header("Способности")]
    [SerializeField] private float cloneCooldown = 15f;
    [SerializeField] private float stealCooldown = 20f;
    [SerializeField] private int maxClones = 3;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float abilityRange = 5f;
    [SerializeField] private float stealDistance = 2f;
    
    [Header("Эффекты")]
    [SerializeField] private GameObject stealEffectPrefab;
    [SerializeField] private GameObject vanishEffectPrefab;
    [SerializeField] private GameObject reappearEffectPrefab;
    
    private TestenemyHealth enemyHealth;
    private Transform currentTarget;
    private float lastCloneTime;
    private float lastStealTime;
    private List<GameObject> activeClones = new List<GameObject>();
    private Vector2 randomDirection;
    private float directionChangeTime;
    private float currentDirectionChangeCooldown;
    private Vector2 spawnPosition;
    private bool isStealing = false;
    private bool hasStolenItem = false;
    private ItemData stolenItem;
    private bool isVanished = false;

    private void Awake()
    {
        enemyHealth = GetComponent<TestenemyHealth>();
        currentDirectionChangeCooldown = Random.Range(2f, 5f);
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (!isServer || enemyHealth.IsDead) return;

        if (isVanished) return;

        FindNearestPlayer();

        if (currentTarget != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, currentTarget.position);

            if (!hasStolenItem)
            {
                // Использование способностей
                if (Time.time > lastCloneTime + cloneCooldown && activeClones.Count < maxClones)
                {
                    CreateClones();
                    lastCloneTime = Time.time;
                }

                if (Time.time > lastStealTime + stealCooldown && distanceToPlayer <= abilityRange)
                {
                    StartCoroutine(StealItemRoutine());
                    lastStealTime = Time.time;
                    return;
                }

                // Движение - держим дистанцию
                if (distanceToPlayer > keepDistance)
                {
                    MoveTowardsPlayer();
                }
                else if (distanceToPlayer < keepDistance - 1f)
                {
                    MoveAwayFromPlayer();
                }
            }
            else
            {
                // После кражи убегаем от игрока
                MoveAwayFromPlayer();
            }
        }
        else
        {
            WanderRandomly();
        }

        CleanUpDestroyedClones();
    }

    [Server]
    private IEnumerator StealItemRoutine()
    {
        isStealing = true;
    
        // Исчезаем
        RpcPlayVanishEffect(transform.position);
        isVanished = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    
        yield return new WaitForSeconds(1f);
    
        // Появляемся рядом с игроком
        Vector2 stealPosition = (Vector2)currentTarget.position + Random.insideUnitCircle.normalized * stealDistance;
        transform.position = stealPosition;
    
        RpcPlayReappearEffect(transform.position);
        isVanished = false;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    
        // Крадем предмет
        PlayerInventory playerInventory = currentTarget.GetComponent<PlayerInventory>();
        if (playerInventory != null)
        {
            stolenItem = playerInventory.StealRandomItem(); // Теперь это работает
            if (stolenItem != null)
            {
                hasStolenItem = true;
                RpcPlayStealEffect(currentTarget.position);
            
                // Клоны разбегаются
                ScatterClones();
            }
        }
    
        isStealing = false;
    }
    [Server]
    private void ScatterClones()
    {
        foreach (GameObject clone in activeClones)
        {
            if (clone != null)
            {
                CloneBehavior cloneBehavior = clone.GetComponent<CloneBehavior>();
                if (cloneBehavior != null)
                {
                    cloneBehavior.ScatterAway(currentTarget.position);
                }
            }
        }
    }

    [Server]
    private void FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance && distance <= detectionRange)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        currentTarget = closestPlayer;
    }

    [Server]
    private void MoveTowardsPlayer()
    {
        if (currentTarget == null) return;

        Vector2 direction = ((Vector2)currentTarget.position - (Vector2)transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        
        // Ограничиваем перемещение в пределах зоны
        if (Vector2.Distance(newPosition, spawnPosition) <= patrolRadius)
        {
            transform.position = newPosition;
            UpdateRotation(direction);
        }
    }

    [Server]
    private void MoveAwayFromPlayer()
    {
        if (currentTarget == null) return;

        Vector2 direction = ((Vector2)transform.position - (Vector2)currentTarget.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        
        // Ограничиваем перемещение в пределах зоны
        if (Vector2.Distance(newPosition, spawnPosition) <= patrolRadius)
        {
            transform.position = newPosition;
            UpdateRotation(direction);
        }
    }

    [Server]
    private void WanderRandomly()
    {
        if (Vector2.Distance(transform.position, spawnPosition) > patrolRadius)
        {
            // Возвращаемся в зону, если вышли за границы
            Vector2 directionToCenter = (spawnPosition - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(directionToCenter * moveSpeed * Time.deltaTime);
            UpdateRotation(directionToCenter);
            return;
        }

        if (Time.time > directionChangeTime)
        {
            randomDirection = Random.insideUnitCircle.normalized;
            directionChangeTime = Time.time + currentDirectionChangeCooldown;
            currentDirectionChangeCooldown = Random.Range(2f, 5f);
        }

        Vector2 newPosition = (Vector2)transform.position + randomDirection * moveSpeed * 0.5f * Time.deltaTime;
        if (Vector2.Distance(newPosition, spawnPosition) <= patrolRadius)
        {
            transform.position = newPosition;
            UpdateRotation(randomDirection);
        }
    }

    private void UpdateRotation(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    [Server]
    private void CreateClones()
    {
        int clonesToCreate = Random.Range(2, 4); // 2-3 клона

        for (int i = 0; i < clonesToCreate; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            GameObject clone = Instantiate(clonePrefab, spawnPos, Quaternion.identity);
            NetworkServer.Spawn(clone);
            activeClones.Add(clone);

            CloneBehavior cloneBehavior = clone.GetComponent<CloneBehavior>();
            if (cloneBehavior != null)
            {
                cloneBehavior.Initialize(transform, this);
            }
        }
    }

    [Server]
    public void Die()
    {
        // Возвращаем украденный предмет при смерти
        if (hasStolenItem && currentTarget != null)
        {
            PlayerInventory playerInventory = currentTarget.GetComponent<PlayerInventory>();
            if (playerInventory != null && stolenItem != null)
            {
                playerInventory.ReturnStolenItem(stolenItem);
            }
        }
        
        // Уничтожаем клонов
        foreach (GameObject clone in activeClones)
        {
            if (clone != null)
            {
                NetworkServer.Destroy(clone);
            }
        }
        activeClones.Clear();
    }

    [ClientRpc]
    private void RpcPlayVanishEffect(Vector3 position)
    {
        if (vanishEffectPrefab != null)
        {
            Instantiate(vanishEffectPrefab, position, Quaternion.identity);
        }
    }

    [ClientRpc]
    private void RpcPlayReappearEffect(Vector3 position)
    {
        if (reappearEffectPrefab != null)
        {
            Instantiate(reappearEffectPrefab, position, Quaternion.identity);
        }
    }

    [ClientRpc]
    private void RpcPlayStealEffect(Vector3 playerPosition)
    {
        if (stealEffectPrefab != null)
        {
            Instantiate(stealEffectPrefab, playerPosition, Quaternion.identity);
        }
    }

    [Server]
    public void CloneDestroyed(GameObject clone)
    {
        if (activeClones.Contains(clone))
        {
            activeClones.Remove(clone);
        }
    }

    [Server]
    private void CleanUpDestroyedClones()
    {
        for (int i = activeClones.Count - 1; i >= 0; i--)
        {
            if (activeClones[i] == null)
            {
                activeClones.RemoveAt(i);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)spawnPosition : transform.position, patrolRadius);
    }
}