using Mirror;
using UnityEngine;
using System.Linq; // Для работы с Random.Range и списками

public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    public EnemyLootTable lootTable;
    public EnemyRank enemyRank = EnemyRank.Common;
    [Range(0f, 1f)] public float dropChance = 0.7f;
    
    [Header("Guaranteed Ingredients")]
    public ItemConfig[] possibleIngredients; // Все возможные ингредиенты
    public int minIngredients = 1; // Минимальное количество ингредиентов
    public int maxIngredients = 3; // Максимальное количество ингредиентов
    
    [Header("Debug")]
    public bool enableLogs = true;

    public void DropItem(int playerLevel)
    {
        if (lootTable == null && enableLogs)
        {
            Debug.LogWarning("LootTable is not assigned!");
            return;
        }

        // Выпадение обычного предмета (если прошёл шанс)
        if (Random.value <= dropChance && lootTable != null)
        {
            EquipmentItemData item = lootTable.GetRandomItem(enemyRank, playerLevel, GetComponent<TestenemyHealth>());
            
            if (item != null && item.Config.Prefab != null)
            {
                Instantiate(item.Config.Prefab, transform.position, Quaternion.identity);
                if (enableLogs) Debug.Log($"Dropped item: {item.Config.itemName}");
            }
            else if (enableLogs)
            {
                Debug.LogWarning(item == null ? "No item was generated" : "Item prefab is missing");
            }
        }
        else if (enableLogs)
        {
            Debug.Log("Drop chance failed for regular item");
        }

        // Гарантированное выпадение случайных ингредиентов
        DropRandomIngredients();
    }

    private void DropRandomIngredients()
    {
        if (possibleIngredients == null || possibleIngredients.Length == 0)
        {
            if (enableLogs) Debug.Log("No possible ingredients defined");
            return;
        }

        // Определяем, сколько ингредиентов выпадет
        int ingredientCount = Random.Range(minIngredients, maxIngredients + 1);
        
        for (int i = 0; i < ingredientCount; i++)
        {
            ItemConfig randomIngredient = possibleIngredients[Random.Range(0, possibleIngredients.Length)];
            
            if (randomIngredient != null && randomIngredient.Prefab != null)
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * 0.5f;
                Instantiate(randomIngredient.Prefab, spawnPos, Quaternion.identity);
                
                if (enableLogs) 
                    Debug.Log($"Dropped ingredient: {randomIngredient.itemName}");
            }
            else if (enableLogs)
            {
                Debug.LogWarning("Ingredient prefab is missing or null");
            }
        }
    }
    
    [Server]
    private void DropItemObj(ItemData itemData)
    {
        Vector3 dropPos = new(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        ItemBase item = ItemFactory.Instance.CreateItemByData(itemData);
        item.gameObject.SetActive(true);
        item.gameObject.transform.position = dropPos;
        NetworkServer.Spawn(item.gameObject);
    }
}