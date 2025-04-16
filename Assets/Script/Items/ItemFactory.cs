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
        ItemBase itemPrefab = itemPrefabs.First(i=>i.itemData.Type==data.Type);
        ItemBase item = Instantiate(itemPrefab);
        item.itemData = data;
        return item;
    }
}
