using UnityEngine;

public class TestenemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _mobLevel = 1;
    
    [Header("Dependencies")]
    [SerializeField] private EnemyLoot _enemyLoot;
    [SerializeField] private PlayerStats _playerStats; 
    
    private int _currentHealth;
    
    private void Start()
    {
        _currentHealth = _maxHealth;
        if (_playerStats == null)
            _playerStats = PlayerStats.Instance; 
    }

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        if (_currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Enemy died!"); 
        if (_enemyLoot != null && _playerStats != null)
        {
            _enemyLoot.DropItem(_playerStats.Lvl);
        }
        else
        {
            Debug.LogWarning($"EnemyLoot: {_enemyLoot}, PlayerStats: {_playerStats}");
        }
        Destroy(gameObject);
    }
}