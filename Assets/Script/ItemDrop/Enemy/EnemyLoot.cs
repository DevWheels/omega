using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    public EnemyLootTable lootTable;
    public EnemyRank enemyRank = EnemyRank.Common;
    [Range(0f, 1f)] public float dropChance = 0.7f;
    public int mobLevel = 1;

    [Header("Debug")]
    public bool enableLogs = true;

    public void DropItem(int playerLevel)
    {
        if (Random.value > dropChance || lootTable == null) 
        {
            if (enableLogs) Debug.Log("Drop failed: chance not passed or lootTable missing");
            return;
        }
        EquipmentItem item = lootTable.GetRandomItem(enemyRank, playerLevel, mobLevel);
        
        if (item == null)
        {
            if (enableLogs) Debug.LogWarning("Failed to get item from lootTable");
            return;
        }
        
        if (item.Config.Prefab != null)
        {
            GameObject droppedPrefab = Instantiate(
                item.Config.Prefab, 
                transform.position, 
                Quaternion.identity
            );
            //
            // ItemWorld itemWorld = droppedPrefab.GetComponent<ItemWorld>();
            // if (itemWorld != null)
            // {
            //     itemWorld.SetItem(item);
            // }

            if (enableLogs) 
            {
                Debug.Log($"Dropped item: {item.Config.itemName}\n" +
                          $"Position: {droppedPrefab.transform.position}\n" +
                          $"Rank: {item.Rank}, Level: {item.Level}\n" +
                          $"Stats: HP={item.Health}, Armor={item.Armor}, ATK={item.Attack}");
            }
        }
        else
        {
            if (enableLogs) Debug.LogWarning($"Item {item.Config.itemName} has no Prefab in config");
        }
    }
}