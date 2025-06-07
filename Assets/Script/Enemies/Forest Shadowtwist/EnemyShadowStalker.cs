using UnityEngine;
using Mirror;
using System.Collections;

public class EnemyShadowStalker : NetworkBehaviour
{
    [Header("Shadow Stalker Settings")]
    [SerializeField] private float stealthDuration = 3f;
    [SerializeField] private float visibleDuration = 2f;
    [SerializeField] private float stealthSpeedMultiplier = 1.5f;
    [SerializeField] private float pounceRange = 5f;
    [SerializeField] private float pounceSpeed = 10f;
    [SerializeField] private float pounceCooldown = 5f;
    [SerializeField] private float postPounceStunDuration = 1f;
    [SerializeField] private float lightVulnerabilityMultiplier = 2f;
    
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f; // Радиус зоны патрулирования
    [SerializeField] private float patrolPointReachedThreshold = 0.5f; // Дистанция для смены точки патрулирования
    [SerializeField] private float idleTimeBetweenPatrolPoints = 2f; // Время простоя между точками
    
    [Header("Visual Effects")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color stealthColor = new Color(1, 1, 1, 0.3f);
    [SerializeField] private GameObject pounceEffectPrefab;
    
    private TestenemyHealth health;
    private Rigidbody2D rb;
    private Animator animator;
    private Coroutine stealthCoroutine;
    private Coroutine patrolCoroutine;
    
    [SyncVar] private bool isInStealth;
    [SyncVar] private bool isPouncing;
    [SyncVar] private bool isStunned;
    [SyncVar] private bool isPatrolling;
    
    private float nextPounceTime;
    private Transform currentTarget;
    private Vector2 lastDirection;
    private Vector2 patrolCenter; // Центральная точка патрулирования
    private Vector2 currentPatrolPoint; // Текущая точка патрулирования

    private void Awake()
    {
        health = GetComponent<TestenemyHealth>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        patrolCenter = transform.position; // Устанавливаем центр патрулирования
        StartStealthCycle();
        StartPatrol();
        health.OnDeath += OnEnemyDeath;
    }

    [Server]
    private void StartPatrol()
    {
        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
        
        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    [Server]
    private IEnumerator PatrolRoutine()
    {
        while (!health.IsDead)
        {
            // Выбираем новую случайную точку в радиусе патрулирования
            currentPatrolPoint = patrolCenter + Random.insideUnitCircle * patrolRadius;
            isPatrolling = true;
            
            // Двигаемся к точке
            while (Vector2.Distance(transform.position, currentPatrolPoint) > patrolPointReachedThreshold && 
                   currentTarget == null && !isStunned)
            {
                Vector2 direction = (currentPatrolPoint - (Vector2)transform.position).normalized;
                lastDirection = direction;
                
                float currentSpeed = isInStealth ? 
                    health.CurrentAttack * stealthSpeedMultiplier * 0.5f : 
                    health.CurrentAttack * 0.5f;
                    
                rb.linearVelocity = direction * currentSpeed;
                RpcUpdateAnimator(direction, rb.linearVelocity.magnitude);
                
                yield return null;
            }
            
            // Останавливаемся на некоторое время
            rb.linearVelocity = Vector2.zero;
            RpcUpdateAnimator(Vector2.zero, 0f);
            isPatrolling = false;
            
            yield return new WaitForSeconds(idleTimeBetweenPatrolPoints);
        }
    }

    [Server]
    private void StartStealthCycle()
    {
        if (stealthCoroutine != null)
            StopCoroutine(stealthCoroutine);
        
        stealthCoroutine = StartCoroutine(StealthCycle());
    }

    [Server]
    private IEnumerator StealthCycle()
    {
        while (!health.IsDead)
        {
            // Enter stealth mode
            isInStealth = true;
            RpcSetStealthVisuals(true);
            yield return new WaitForSeconds(stealthDuration);
            
            // Exit stealth mode
            isInStealth = false;
            RpcSetStealthVisuals(false);
            yield return new WaitForSeconds(visibleDuration);
        }
    }

    [ClientRpc]
    private void RpcSetStealthVisuals(bool stealth)
    {
        if (stealth)
        {
            spriteRenderer.color = stealthColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    [Server]
    void Update()
    {
        if (health.IsDead || isStunned) return;
        
        FindNearestTarget();
        
        if (currentTarget != null)
        {
            // Проверяем, находится ли игрок в зоне патрулирования
            if (Vector2.Distance(patrolCenter, currentTarget.position) <= patrolRadius)
            {
                HandleMovement();
                HandlePounce();
            }
            else
            {
                // Игрок вышел из зоны - возвращаемся к патрулированию
                currentTarget = null;
                if (!isPatrolling)
                {
                    StartPatrol();
                }
            }
        }
    }

    [Server]
    private void FindNearestTarget()
    {
        if (currentTarget != null) return;
        
        PlayerStats[] players = FindObjectsOfType<PlayerStats>();
        float closestDistance = float.MaxValue;
        Transform closestPlayer = null;
        
        foreach (PlayerStats player in players)
        {
            if (player.greenZone) continue;
            
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance && 
                Vector2.Distance(patrolCenter, player.transform.position) <= patrolRadius)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }
        
        currentTarget = closestPlayer;
        if (currentTarget != null && patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            isPatrolling = false;
        }
    }

    [Server]
    private void HandleMovement()
    {
        if (isPouncing) return;
        
        Vector2 direction = (currentTarget.position - transform.position).normalized;
        lastDirection = direction;
        
        float currentSpeed = isInStealth ? 
            health.CurrentAttack * stealthSpeedMultiplier : 
            health.CurrentAttack;
            
        rb.linearVelocity = direction * currentSpeed;
        
        RpcUpdateAnimator(direction, rb.linearVelocity.magnitude);
    }

    [Server]
    private void HandlePounce()
    {
        if (Time.time < nextPounceTime || isInStealth) return;
        
        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);
        if (distanceToTarget <= pounceRange)
        {
            StartCoroutine(PounceAttack());
        }
    }

    [Server]
    private IEnumerator PounceAttack()
    {
        isPouncing = true;
        nextPounceTime = Time.time + pounceCooldown;
        
        // Store original speed and set pounce speed
        Vector2 originalVelocity = rb.linearVelocity;
        rb.linearVelocity = lastDirection * pounceSpeed;
        
        RpcPlayPounceEffect();
        
        yield return new WaitForSeconds(0.5f); // Pounce duration
        
        // Stun after pounce
        isStunned = true;
        isPouncing = false;
        rb.linearVelocity = Vector2.zero;
        
        yield return new WaitForSeconds(postPounceStunDuration);
        
        isStunned = false;
    }

    [ClientRpc]
    private void RpcPlayPounceEffect()
    {
        if (pounceEffectPrefab != null)
        {
            Instantiate(pounceEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    [ClientRpc]
    private void RpcUpdateAnimator(Vector2 direction, float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", speed);
        }
    }

    [Server]
    public void TakeDamageWithLightCheck(int damage, PlayerStats attacker, bool isLightDamage = false)
    {
        int finalDamage = isLightDamage ? 
            Mathf.FloorToInt(damage * lightVulnerabilityMultiplier) : 
            damage;
        
        health.TakeDamage(finalDamage, attacker);
        
        // Interrupt stealth when hit
        if (isInStealth)
        {
            if (stealthCoroutine != null)
                StopCoroutine(stealthCoroutine);
            
            isInStealth = false;
            RpcSetStealthVisuals(false);
            StartCoroutine(DelayedStealthRestart());
        }
    }

    [Server]
    private IEnumerator DelayedStealthRestart()
    {
        yield return new WaitForSeconds(2f);
        StartStealthCycle();
    }

    [Server]
    private void OnEnemyDeath()
    {
        if (stealthCoroutine != null)
            StopCoroutine(stealthCoroutine);
        
        if (patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
    }

    [Server]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isServer || !isPouncing) return;
        
        PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.TakeHit(health.CurrentAttack);
            
            // Apply root effect to player
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ApplyRoot(0.5f);
            }
        }
    }

    // Визуализация зоны патрулирования в редакторе
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, patrolRadius);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(patrolCenter, patrolRadius);
            
            if (isPatrolling)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, currentPatrolPoint);
                Gizmos.DrawSphere(currentPatrolPoint, 0.3f);
            }
        }
    }
}