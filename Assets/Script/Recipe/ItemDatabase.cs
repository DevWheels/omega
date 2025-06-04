using UnityEngine;
using System.Collections.Generic;

public static class ItemDatabase
{
    private static Dictionary<string, ItemConfig> _items;
    private static bool _isInitialized = false;

    private static void InitializeIfNeeded()
    {
        if (_isInitialized) return;
        
        _items = new Dictionary<string, ItemConfig>();
        
        // Загружаем все конфиги предметов из папки Resources/Items
        ItemConfig[] allItems = Resources.LoadAll<ItemConfig>("Items");
        
        if (allItems == null || allItems.Length == 0)
        {
            Debug.LogWarning("No items found in Resources/Items folder!");
            return;
        }

        foreach (var item in allItems)
        {
            if (item != null)
            {
                string normalizedKey = NormalizeName(item.name);
                _items[normalizedKey] = item;
                Debug.Log($"Registered item: {item.name} (normalized: {normalizedKey})");
            }
        }

        _isInitialized = true;
        Debug.Log($"ItemDatabase initialized with {_items.Count} items.");
    }

    private static string NormalizeName(string name)
    {
        return name.Replace(" ", "").ToLower();
    }

    public static ItemConfig GetItemById(string id)
    {
        InitializeIfNeeded();
        
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Item ID cannot be null or empty!");
            return null;
        }

        string normalizedId = NormalizeName(id);
        
        if (_items.TryGetValue(normalizedId, out var item))
        {
            return item;
        }

        Debug.LogError($"Item '{id}' not found. Available items:");
        foreach (var name in _items.Keys)
        {
            Debug.Log($"- '{name}'");
        }
        return null;
    }
}