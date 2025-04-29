using Mirror;
using UnityEngine;

public class StationaryEnemyAttack : NetworkBehaviour
{
    [Header("Настройки атаки")]
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackCooldown = 2f; 
    [SerializeField] private int _attackDamage = 10;        
    [SerializeField] private string _playerTag = "Player"; 
    
    private TestenemyHealth _enemyHealth;                    
    private float _lastAttackTime;                       
    private Transform _currentTarget;                       
    
    private void Awake()
    {
        _enemyHealth = GetComponent<TestenemyHealth>();
    }
    
    private void Update()
    {

        if (!isServer || _enemyHealth.IsDead) return;
        

        FindNearestPlayerByTag();
        

        if (_currentTarget != null && CanAttack())
        {
            Attack();
        }
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
                if (distance < closestDistance && distance <= _attackRange)
                {
                    closestDistance = distance;
                    closestPlayer = player.transform;
                }
            }
        }
        
        _currentTarget = closestPlayer;
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
        if (_currentTarget == null) return;
        

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}