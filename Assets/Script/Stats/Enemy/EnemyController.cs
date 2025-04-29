using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TestenemyHealth), typeof(Animator))]
public class EnemyController : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stoppingDistance = 1.5f;

    [Header("Patrol Settings")]
    [SerializeField] private bool shouldPatrol = true;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 2f;


    [Header("Attack Settings")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRadius = 0.5f; 
    [SerializeField] private LayerMask attackLayerMask;

    [Header("Vision Settings")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float visionAngle = 90f;

    private Rigidbody2D rb;
    private TestenemyHealth enemyHealth;
    private Animator animator;
    private Transform player;
    private int currentPatrolIndex = 0;
    private float waitCounter;
    private bool isWaiting = false;
    private float lastAttackTime;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<TestenemyHealth>();
        animator = GetComponent<Animator>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        InitializeEnemy();
    }
    [Server]
    private void InitializeEnemy()
    {
        enemyHealth.enabled = true;
        rb.simulated = true;
        FindPlayer();
    
        enemyHealth.OnStartServer(); 
    
        Debug.Log("Enemy initialized on server");
    }
    [ServerCallback]
    private void Update()
    {
        if (player == null) 
        {
            FindPlayer();
            return;
        }


        bool canSeePlayer = CanSeePlayer();
        bool playerInRange = IsPlayerInRange();

        if (canSeePlayer && playerInRange)
        {
           
            if (Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                if (CanAttackPlayer())
                {
                    Attack();
                }
            }
            else 
            {
                ChasePlayer();
            }
        }
        else if (shouldPatrol)
        {
            Patrol();
        }
        else
        {
            StopMovement();
        }
    }

    [Server]
    private void FindPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0) 
        {

            GameObject closestPlayer = null;
            float minDistance = float.MaxValue;
        
            foreach (var playerObj in players)
            {
                float dist = Vector2.Distance(transform.position, playerObj.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestPlayer = playerObj;
                }
            }
        
            player = closestPlayer.transform;
            Debug.Log("Player found!");
        }
        else
        {
            player = null;
        }
    }


    [Server]
    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
    

        float angleToPlayer = Vector2.Angle(GetFacingDirection(), directionToPlayer);
        if (angleToPlayer > visionAngle / 2f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            directionToPlayer, 
            detectionRange, 
            ~obstacleLayers
        );

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, directionToPlayer * hit.distance, Color.green, 1f);
            return true;
        }
        return false;
    }
    void OnGUI()
    {
        if (isServer)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Enemy State: Player={(player != null)}");
            if (player != null)
            {
                GUI.Label(new Rect(10, 30, 300, 20), $"Distance: {Vector2.Distance(transform.position, player.position)}");
            }
        }
    }
    private Vector2 GetFacingDirection()
    {
        return isFacingRight ? Vector2.right : Vector2.left;
    }

    [Server]
    private bool IsPlayerInRange()
    {
        if (player == null) return false;
        float distance = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Distance to player: {distance}");
        return distance <= detectionRange;
    }

    [Server]
    private bool CanAttackPlayer()
    {
        if (player == null) 
        {
            Debug.Log("Player is null");
            return false;
        }
    
        float distance = Vector2.Distance(transform.position, player.position);
        bool inAttackRange = distance <= attackRange;
        bool canAttack = Time.time - lastAttackTime >= attackCooldown;
    
        Debug.Log($"Attack check - Distance: {distance}, InRange: {inAttackRange}, CooldownReady: {canAttack}");
        return inAttackRange && canAttack;
    }

    [Server]
    private void Attack()
    {
        if (!enemyHealth.CanAttack()) return;

        lastAttackTime = Time.time;
        RpcPlayAttackAnimation();
        enemyHealth.ResetAttackCooldown();


        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayerMask);
        
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            
            PlayerStats playerStats = hit.GetComponent<PlayerStats>();
            if (playerStats != null && !playerStats.isLocalPlayer)
            {
                playerStats.TakeHit(attackDamage);
                Debug.Log($"Dealt {attackDamage} damage to player {playerStats.name}");
            }
        }
    }
    [ClientRpc]
    private void RpcPlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRadius); // Радиус атаки
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        if (shouldPatrol && patrolPoints != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    if (i < patrolPoints.Length - 1 && patrolPoints[i+1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i+1].position);
                    }
                }
            }
        }
    }
    [Server]
    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        if (distanceToPlayer > stoppingDistance)
        {
            rb.linearVelocity = direction * moveSpeed;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            StopMovement();
        }
    }

    [Server]
    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) isWaiting = false;
            return;
        }

        Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }

        if (Vector2.Distance(transform.position, targetPosition) > 0.5f)
        {
            rb.linearVelocity = direction * moveSpeed;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            StopMovement();
            isWaiting = true;
            waitCounter = waitTimeAtPoint;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    [Server]
    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    [Server]
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

   
}