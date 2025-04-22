using UnityEngine;

public class TestenemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _mobLevel = 1;
    
    [Header("Dependencies")]
    [SerializeField] private EnemyLoot _enemyLoot;
    
    private int _currentHealth;
    private PlayerStats _lastAttacker;
    
    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage, PlayerStats attacker)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        _lastAttacker = attacker; 
        
        if (_currentHealth <= 0) 
            Die();
    }

    private void Die()
    {
        Debug.Log("Enemy died! Killer: " + (_lastAttacker != null ? _lastAttacker.name : "unknown"));
        
        if (_enemyLoot != null)
        {
            if (_lastAttacker != null)
            {
                _enemyLoot.DropItem(_lastAttacker.Lvl);
            }
            else
            {
                Debug.LogWarning("No attacker registered, using default level");
                _enemyLoot.DropItem(1); 
            }
        }
        else
        {
            Debug.LogWarning("EnemyLoot reference is missing!");
        }
        
        Destroy(gameObject);
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     var playerAttack = other.GetComponent<PlayerAttack>();
    //     if (playerAttack != null && playerAttack.PlayerStats != null)
    //     {
    //         TakeDamage(playerAttack.damage, playerAttack.PlayerStats);
    //     }
    // }
}