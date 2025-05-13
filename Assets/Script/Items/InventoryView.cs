using UnityEngine;

public class InventoryView : MonoBehaviour {
    [SerializeField]
    private InventorySlotView[] inventorySlots = new InventorySlotView[24];

    public void SetData(PlayerInventory playerInventory) {
        for (int index = 0; index < playerInventory.Slots.Count; index++) {
            InventorySlot var = playerInventory.Slots[index];
            if (var != null && var.ItemConfig != null) {
                inventorySlots[index].PutInSlot(var.ItemConfig, var.ItemData);
            } else {
                inventorySlots[index].ClearSlot();
            }
        }
    }

    public void PutInEmptySlot(ItemConfig itemConfig, ItemData itemData) {
        for (int i = 0; i < inventorySlots.Length; i++) {
            if (inventorySlots[i].slotItemConfig == null) {
                inventorySlots[i].PutInSlot(itemConfig,itemData);
                return;
            }
        }
    }
}