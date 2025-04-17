using System;
using System.Collections.Generic;
using Mirror;

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
}

[Serializable]
public class InventorySlot {
    public ItemConfig ItemConfig;
    public ItemData ItemData;
}