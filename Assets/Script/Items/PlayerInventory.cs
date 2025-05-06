using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInventory : NetworkBehaviour {
    public List<InventorySlot> Slots = new List<InventorySlot>(24);
    public bool HasItems(PickItemType type, int amount)
    {
        int count = 0;
        foreach (var slot in Slots)
        {
            if (slot.ItemData != null && slot.ItemData.Type == type)
            {
                count++;
                if (count >= amount) return true;
            }
        }
        return false;
    }
    [Command]
    public void RemoveItems(PickItemType type, int amount)
    {
        int remaining = amount;
        for (int i = 0; i < Slots.Count && remaining > 0; i++)
        {
            if (Slots[i].ItemData != null && Slots[i].ItemData.Type == type)
            {
                Slots[i].ItemConfig = null;
                Slots[i].ItemData = null;
                remaining--;
            }
        }
    }
    
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
        NetworkServer.Spawn(item.gameObject);
    }
}

[Serializable]
public class InventorySlot {
    public ItemConfig ItemConfig;
    public ItemData ItemData;
}