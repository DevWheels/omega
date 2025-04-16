using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Transform SlotsParent;
    public bool isOpened;
    private InventorySlot[] inventorySlots = new InventorySlot[24];

    private void Start()
    {
        instance = this;
        if (SlotsParent != null)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i] = SlotsParent.GetChild(i).GetComponent<InventorySlot>();
            }
        }
    }

    public void PutInEmptySlot(ItemConfig itemConfig,ItemData itemData)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].slotItemConfig == null)
            {
                inventorySlots[i].PutInSlot(itemConfig,itemData); 
                return;
            }
        }
    }
}