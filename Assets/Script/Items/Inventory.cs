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


    public bool CanAddItem(Item1 item)
    {

        if (item.isStackable)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.SlotItem != null &&
                    slot.SlotItem.Name == item.Name &&  //Макс itemName на Name
                    slot.amount < item.maxStackAmount)
                {
                    return true;
                }
            }
        }


        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.SlotItem == null)
            {
                return true;
            }
        }

        return false;
    }


    public void AddItem(Item1 item)
    {

        if (item.isStackable)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.SlotItem != null &&
                    slot.SlotItem.Name == item.Name &&   //Макс itemName на Name
                    slot.amount < item.maxStackAmount)
                {
                    slot.amount++;
                    slot.UpdateAmountText();
                    return;
                }
            }
        }


        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].SlotItem == null)
            {
                inventorySlots[i].PutInSlot(item);
                return;
            }
        }

        Debug.LogWarning("Инвентарь полон!");
    }

    public void PutInEmptySlot(Item1 item, GameObject obj)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].SlotItem == null)
            {
                inventorySlots[i].PutInSlot(item);  //Макс obj убрал
                return;
            }
        }
    }
}