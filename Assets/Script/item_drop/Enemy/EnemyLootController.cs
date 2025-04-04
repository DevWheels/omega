using UnityEngine;

public class EnemyLootController : MonoBehaviour
{
    public EnemyLootTable lootTable;
    public EnemyRank enemyRank;
    public int mobLevel = 1;
    [Range(0f, 1f)] public float dropChance = 0.7f;

    public void DropItem(int playerLevel)
    {
        if (Random.value > dropChance || lootTable == null) return;

        EquipmentItem item = lootTable.GetRandomItem(enemyRank, playerLevel, mobLevel);
        if (item != null && item.worldPrefab != null)
        {
            Instantiate(item.worldPrefab, transform.position, Quaternion.identity);
            item.LogStats();
        }
    }
}