using UnityEngine;
using System.Collections.Generic;

public static class RecipeDatabase
{
    private static Dictionary<string, ItemRecipe> _recipes;
    private static bool _isInitialized = false;

    private static void InitializeIfNeeded()
    {
        if (_isInitialized) return;
        
        _recipes = new Dictionary<string, ItemRecipe>();
        
        // Загружаем все рецепты из папки Resources/Recipes
        ItemRecipe[] allRecipes = Resources.LoadAll<ItemRecipe>("Recipes");
        
        if (allRecipes == null || allRecipes.Length == 0)
        {
            Debug.LogWarning("No recipes found in Resources/Recipes folder!");
            return;
        }

        foreach (var recipe in allRecipes)
        {
            if (recipe != null)
            {
                // Проверяем ссылки на предметы
                if (recipe.resultItem == null)
                {
                    Debug.LogError($"Recipe '{recipe.name}' has no result item assigned!");
                    continue;
                }

                // Автоматически подгружаем конфиг предмета
                recipe.resultItem = ItemDatabase.GetItemById(recipe.resultItem.name);
                
                string normalizedKey = NormalizeName(recipe.name);
                _recipes[normalizedKey] = recipe;
                Debug.Log($"Registered recipe: {recipe.name}");
            }
        }

        _isInitialized = true;
        Debug.Log($"RecipeDatabase initialized with {_recipes.Count} recipes.");
    }

    private static string NormalizeName(string name)
    {
        return name.Replace(" ", "").ToLower();
    }

    public static ItemRecipe GetRecipeById(string id)
    {
        InitializeIfNeeded();
        
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Recipe ID cannot be null or empty!");
            return null;
        }

        string normalizedId = NormalizeName(id);
        
        if (_recipes.TryGetValue(normalizedId, out var recipe))
        {
            return recipe;
        }

        Debug.LogError($"Recipe '{id}' not found. Available recipes:");
        foreach (var name in _recipes.Keys)
        {
            Debug.Log($"- '{name}'");
        }
        return null;
    }
}