using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemFactory : MonoBehaviour {
    public static ItemFactory Instance;
    public List<ItemBase> itemPrefabs;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject manaPotionPrefab;
    [SerializeField] private GameObject bootsPrefab;
    [SerializeField] private GameObject staffPrefab;
    [SerializeField] private GameObject chestplatePrefab;
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemBase CreateItemByType(PickItemType type)
    {
        GameObject prefab = type switch
        {
            PickItemType.Apple => applePrefab,
            PickItemType.ManaPotion => manaPotionPrefab,
            PickItemType.Boots => bootsPrefab,
            PickItemType.Staff => staffPrefab,
            PickItemType.Chestplate => chestplatePrefab,
            _ => null
        };

        if (prefab == null) return null;

        var itemObj = Instantiate(prefab);
        var item = itemObj.GetComponent<ItemBase>();
        return item;
    }
    //public ItemBase CreateItemByData(ItemData data) {
     //   ItemBase itemPrefab = itemPrefabs.First(i=>i.itemData.Type==data.Type);
     //   ItemBase item = Instantiate(itemPrefab);
     //   item.itemData = data;
     //   return item;
   // }
    public ItemBase CreateItemByData(ItemData itemData)
    {
        return CreateItemByType(itemData.Type);
    }
}