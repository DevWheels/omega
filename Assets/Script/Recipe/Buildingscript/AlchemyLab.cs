using UnityEngine;
using Mirror;

public class AlchemyLab : Building
{
    [Header("Crafting Settings")]
    [SerializeField] private ItemRecipe[] availableRecipes;
    [SerializeField] private float craftRadius = 3f;
    
    protected override void SetupUI(GameObject uiPanel)
    {
        if (uiPanel == null) return;
        
        var smithyUI = uiPanel.GetComponent<SmithyUI>();
        if (smithyUI != null)
        {
            smithyUI.Initialize(this, availableRecipes);
        }
    }
    
    public override bool CanCraft(ItemRecipe recipe, PlayerInventory inventory)
    {
        if (recipe == null || inventory == null)
        {
            Debug.LogWarning($"CanCraft check failed: {(recipe == null ? "Recipe is null" : "Inventory is null")}");
            return false;
        }

        if (recipe.requiredBuildingLevel > buildingLevel)
        {
            Debug.Log($"Building level too low. Required: {recipe.requiredBuildingLevel}, Current: {buildingLevel}");
            return false;
        }
        
        if (!IsPlayerInRange())
        {
            Debug.Log("Player is too far from the building");
            return false;
        }
        
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient == null || ingredient.itemConfig == null)
            {
                Debug.LogWarning("Ingredient or itemConfig is null in recipe");
                return false;
            }

            if (!inventory.HasItem(ingredient.itemConfig, ingredient.amount))
            {
                Debug.Log($"Missing ingredient: {ingredient.amount}x {ingredient.itemConfig.name}");
                return false;
            }
        }
        
        return true;
    }
    
    public override void CraftItem(ItemRecipe recipe, PlayerInventory inventory)
    {
        if (!CanCraft(recipe, inventory)) 
        {
            Debug.LogWarning("Crafting conditions not met!");
            return;
        }

        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient != null && ingredient.itemConfig != null)
            {
                inventory.CmdRemoveItem(ingredient.itemConfig, ingredient.amount);
            }
        }

        if (recipe.resultItem != null)
        {
            var itemData = new ItemData {
                configId = recipe.resultItem.name,
                rank = CalculateResultRank(recipe)
            };
            inventory.PutInEmptySlot(recipe.resultItem, itemData);
            Debug.Log($"Successfully crafted: {recipe.resultItem.itemName}");
        }
        
        PlayCraftEffect();
    }

    private ItemRank CalculateResultRank(ItemRecipe recipe)
    {
        return ItemRank.C;
    }

    private bool IsPlayerInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, craftRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != null && 
                hitCollider.CompareTag("Player") && 
                hitCollider.GetComponent<NetworkIdentity>()?.isLocalPlayer == true)
            {
                return true;
            }
        }
        return false;
    }
}