using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour {
    public List<InventorySlot> Slots = new List<InventorySlot>(24);

    public void PutInEmptySlot(ItemConfig itemConfig, ItemData itemData) {
        for (int i = 0; i < Slots.Count; i++) {
            if (Slots[i].ItemConfig != null) {
                continue;
            }

            Slots[i] = new InventorySlot {
                ItemConfig = itemConfig,
                ItemData = itemData
            };
            return;
        }
    }

    public void DropOnDie() {
        foreach (InventorySlot t in Slots) {
            if (t.ItemConfig == null) {
                continue;
            }
            DropItem(t.ItemData);
            t.ItemConfig = null;
            t.ItemData = null;
        }
    }
    [Command]
    private void DropItem(ItemData itemData) {
        Vector3 dropPos = new(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        ItemBase item = ItemFactory.Instance.CreateItemByData(itemData);

        item.gameObject.SetActive(true);
        item.gameObject.transform.position = dropPos;
    }
}

[Serializable]
public class InventorySlot {
    public ItemConfig ItemConfig;
    public ItemData ItemData;
}