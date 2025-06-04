using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemFactory : MonoBehaviour {
    public static ItemFactory Instance;
    public List<ItemBase> itemPrefabs;
    private void Awake() {
        Instance = this;
    }

    public ItemBase CreateItemByData(ItemData data) {
        ItemBase itemPrefab = itemPrefabs.FirstOrDefault(i => i.itemData.Type == data.Type);
    
        if (itemPrefab == null) {
            Debug.LogError($"Prefab for item type {data.Type} not found in ItemFactory!");
            return null;
        }

        ItemBase item = Instantiate(itemPrefab);
        item.itemData = data;
        item.itemConfig = Resources.Load<ItemConfig>(data.configId);
        return item;
    }
}