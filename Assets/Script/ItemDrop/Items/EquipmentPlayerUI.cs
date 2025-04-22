using System.Collections.Generic;
using UnityEngine;

public class EquipmentPlayerUI : MonoBehaviour{

    public List<EquipmentItemConfig> GetPlayerEquipmentByType(ItemType type)
    {
        if (PlayerEquipment.Instance == null)
        {
            Debug.LogError("PlayerEquipment instance is missing!");
            return new List<EquipmentItemConfig>();
        }

        return PlayerEquipment.Instance.PlayerInventory.FindAll(item => 
            item != null && item.itemType == type);
    }

 
    public void LogEquipmentByType(ItemType type)
    {
        var items = GetPlayerEquipmentByType(type);
        Debug.Log($"Found {items.Count} items of type {type}:");
        
        foreach (var item in items)
        {
            if (item != null)
            {
                Debug.Log(item.name);
            }
        }
    }
}
