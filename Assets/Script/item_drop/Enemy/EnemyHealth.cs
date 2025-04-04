using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int mobLevel = 1;
    public event System.Action OnDeath;

    private int currentHealth;

    private void Start() => currentHealth = maxHealth;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{name} took {damage} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();

        int playerLevel = PlayerStats.Instance?.Lvl ?? 1;

        GetComponent<EnemyLoot>().DropItem(playerLevel);

        Destroy(gameObject);
    }

    private int GetPlayerLevel()
    {
        return PlayerStats.Instance != null ? PlayerStats.Instance.Lvl : 1;
    }
}