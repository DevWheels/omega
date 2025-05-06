using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AlchemyBuilding : NetworkBehaviour
{
    public List<AlchemyRecipe> Recipes;

    [Command(requiresAuthority = false)]
    public void CmdCraftPotion(string recipeId, NetworkConnectionToClient sender = null)
    {
        var recipe = Recipes.Find(r => r.RecipeId == recipeId);
        if (recipe == null) return;

        var player = sender.identity.GetComponent<PlayerInventory>();
        var playerBuildings = sender.identity.GetComponent<PlayerBuildings>();
        var alchemyLevel = playerBuildings.GetBuildingLevel("alchemy");

        if (recipe.RequiredBuildingLevel > alchemyLevel) return;

        foreach (var ingredient in recipe.Ingredients)
        {
            if (!player.HasItems(ingredient.ItemType, ingredient.Amount))
            {
                return;
            }
        }

        foreach (var ingredient in recipe.Ingredients)
        {
            player.RemoveItems(ingredient.ItemType, ingredient.Amount);
        }

        var resultItem = ItemFactory.Instance.CreateItemByType(recipe.ResultItem);
        player.PutInEmptySlot(resultItem.itemConfig, resultItem.itemData);
    }
}

[System.Serializable]
public class AlchemyRecipe
{
    public string RecipeId;
    public string DisplayName;
    public int RequiredBuildingLevel;
    public PickItemType ResultItem;
    public List<ItemRequirement> Ingredients;
}

[System.Serializable]
public class ItemRequirement
{
    public PickItemType ItemType;
    public int Amount;
}