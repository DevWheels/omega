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
        
            if (Slots[i].SlotView != null) {
                Slots[i].SlotView.PutInSlot(itemConfig, itemData);
            }
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
    
    public bool HasItem(ItemConfig itemConfig, int amount)
    {
        if (itemConfig == null)
        {
            Debug.LogWarning("HasItem: itemConfig is null");
            return false;
        }

        int count = 0;
        foreach (var slot in Slots)
        {
            if (slot.ItemConfig == itemConfig)
            {
                count++;
                if (count >= amount) 
                    return true;
            }
        }
        return false;
    }
    [Command]
    public void CmdRemoveSpecificItems(ItemConfig itemConfig, int amount, ItemRank minRank)
    {
        int remaining = amount;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].ItemConfig == itemConfig && Slots[i].ItemData.rank >= minRank)
            {
                Slots[i] = new InventorySlot(); 
                remaining--;
                if (remaining <= 0) break;
            }
        }
        RpcUpdateInventory();
    }
    
    [Command]
    public void CmdCraftItem(ItemRecipe recipe)
    {
        if (!CanCraft(recipe)) return;

        foreach (var ingredient in recipe.ingredients)
        {
            CmdRemoveSpecificItems(ingredient.itemConfig, ingredient.amount, ingredient.minRank);
        }

        var itemData = new ItemData {
            configId = recipe.resultItem.name,
            rank = CalculateResultRank(recipe)
        };
        PutInEmptySlot(recipe.resultItem, itemData);
        
        RpcUpdateInventory();
    }
    public bool CanCraft(ItemRecipe recipe)
    {
        Debug.Log($"Checking recipe: {recipe?.name ?? "NULL"}");
        foreach (var ing in recipe?.ingredients ?? Array.Empty<Ingredient>())
        {
            Debug.Log($"Ingredient: {ing?.amount}x {ing?.itemConfig?.name ?? "NULL"}");
        }
        foreach (var ingredient in recipe.ingredients)
        {
            
            if (!HasItem(ingredient.itemConfig, ingredient.amount))
                return false;
        }
        return true;
    }
    private ItemRank CalculateResultRank(ItemRecipe recipe)
    {
        return ItemRank.C;
    }
    
    [Command]
    public void CmdRemoveItem(ItemConfig itemConfig, int amount)
    {
        if (itemConfig == null) return;

        int remaining = amount;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].ItemConfig == itemConfig)
            {
                Slots[i] = new InventorySlot(); 
                remaining--;
                if (remaining <= 0) break;
            }
        }
        RpcUpdateInventory();
    }

    [ClientRpc]
    private void RpcUpdateInventory()
    {
        InventoryManager.Instance.InventoryView.SetData(this);
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
    [NonSerialized]
    public InventorySlotView SlotView;
}