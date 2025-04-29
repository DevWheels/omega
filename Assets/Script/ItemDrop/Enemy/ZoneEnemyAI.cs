using Mirror;
using UnityEngine;

public class ZoneEnemyAI : NetworkBehaviour
{
    [Header("Настройки передвижения")]
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _minIdleTime = 1f;
    [SerializeField] private float _maxIdleTime = 3f;
    
    [Header("Настройки атаки")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private string _playerTag = "Player";
    
    private TestenemyHealth _enemyHealth;
    private float _lastAttackTime;
    private Transform _currentTarget;
    private Vector3 _startPosition;
    private Vector3 _currentPatrolPoint;
    private float _idleTimer;
    private float _currentIdleTime;
    private bool _isChasing;

    private void Awake()
    {
        _enemyHealth = GetComponent<TestenemyHealth>();
        _startPosition = transform.position;
        SetNewPatrolPoint();
    }

    private void Update()
    {
        if (!isServer || _enemyHealth.IsDead) return;

        FindNearestPlayerByTag();

        if (_currentTarget != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _currentTarget.position);
            

            if (distanceToPlayer <= _attackRange)
            {
                Attack();
                return;
            }
            

            if (distanceToPlayer <= _patrolRadius * 1.5f) 
            {
                ChasePlayer();
                _isChasing = true;
                return;
            }
        }


        _isChasing = false;
        Patrol();
    }

    [Server]
    private void FindNearestPlayerByTag()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(_playerTag);
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject player in players)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance < closestDistance && distance <= _patrolRadius * 1.5f)
                {
                    closestDistance = distance;
                    closestPlayer = player.transform;
                }
            }
        }

        _currentTarget = closestPlayer;
    }

    [Server]
    private void Patrol()
    {

        if (Vector3.Distance(transform.position, _currentPatrolPoint) < 0.5f)
        {
            _idleTimer += Time.deltaTime;
            
            if (_idleTimer >= _currentIdleTime)
            {
                SetNewPatrolPoint();
                _idleTimer = 0f;
                _currentIdleTime = Random.Range(_minIdleTime, _maxIdleTime);
            }
        }
        else
        {

            Vector3 direction = (_currentPatrolPoint - transform.position).normalized;
            transform.position += direction * _moveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(_currentPatrolPoint.x, transform.position.y, _currentPatrolPoint.z));
        }
    }

    [Server]
    private void ChasePlayer()
    {
        if (_currentTarget == null) return;


        Vector3 toPlayer = _currentTarget.position - _startPosition;
        if (toPlayer.magnitude > _patrolRadius)
        {
            toPlayer = toPlayer.normalized * _patrolRadius;
            _currentTarget = null;
            return;
        }

        Vector3 direction = (_currentTarget.position - transform.position).normalized;
        transform.position += direction * _moveSpeed * Time.deltaTime;
        transform.LookAt(new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z));
    }

    [Server]
    private void SetNewPatrolPoint()
    {
 
        Vector2 randomPoint = Random.insideUnitCircle * _patrolRadius;
        _currentPatrolPoint = _startPosition + new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    [Server]
    private bool CanAttack()
    {
        return _currentTarget != null && 
               Vector3.Distance(transform.position, _currentTarget.position) <= _attackRange &&
               Time.time > _lastAttackTime + _attackCooldown;
    }

    [Server]
    private void Attack()
    {
        if (!CanAttack()) return;

        transform.LookAt(new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z));
        
        PlayerStats playerStats = _currentTarget.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeHit(_attackDamage);
            _lastAttackTime = Time.time;
            RpcPlayAttackEffects();
        }
    }

    [ClientRpc]
    private void RpcPlayAttackEffects()
    {

    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_startPosition, _patrolRadius);
        

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}