using Mirror;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    public EnemyLootTable lootTable;
    public EnemyRank enemyRank = EnemyRank.Common;
    [Range(0f, 1f)] public float dropChance = 0.7f;
    [Header("Debug")]
    public bool enableLogs = true;

    public void DropItem(int playerLevel)
    {
        if (lootTable == null)
        {
            if (enableLogs) Debug.LogWarning("LootTable is not assigned!");
            return;
        }

        if (Random.value > dropChance)
        {
            if (enableLogs) Debug.Log("Drop chance failed");
            return;
        }

        EquipmentItemData item = lootTable.GetRandomItem(enemyRank, playerLevel, GetComponent<TestenemyHealth>());
        
        if (item == null)
        {
            if (enableLogs) Debug.LogWarning("No item was generated");
            return;
        }

        if (item.Config.Prefab != null)
        {
            Instantiate(item.Config.Prefab, transform.position, Quaternion.identity);
            if (enableLogs) Debug.Log($"Dropped item: {item.Config.itemName}");
        }
        else
        {
            if (enableLogs) Debug.LogWarning("Item prefab is missing");
        }
    }
    
    [Server]
    private void DropItemObj(ItemData itemData) {
        Vector3 dropPos = new(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        ItemBase item = ItemFactory.Instance.CreateItemByData(itemData);
        item.gameObject.SetActive(true);
        item.gameObject.transform.position = dropPos;
        NetworkServer.Spawn(item.gameObject);
    }
}