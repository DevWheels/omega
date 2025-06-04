using UnityEngine;
using Mirror;

public class AlchemyLab : Building
{
    [Header("Crafting Settings")]
    [SerializeField] private ItemRecipe[] availableRecipes;
    [SerializeField] private float craftRadius = 3f;
    
    
    private void Start()
    {
        if (availableRecipes != null && availableRecipes.Length > 0)
        {
            var testRecipe = RecipeDatabase.GetRecipeById(availableRecipes[0].name);
        }
    }
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
            Debug.LogWarning("Recipe or Inventory is null!");
            return false;
        }

        // Проверка уровня здания
        if (recipe.requiredBuildingLevel > buildingLevel)
        {
            Debug.Log($"Building level too low. Required: {recipe.requiredBuildingLevel}, Current: {buildingLevel}");
            return false;
        }

        // Подробная проверка ингредиентов
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient == null || ingredient.itemConfig == null)
            {
                Debug.LogWarning("Ingredient or itemConfig is null!");
                return false;
            }

            if (!inventory.HasItem(ingredient.itemConfig, ingredient.amount, ingredient.minRank))
            {
                Debug.Log($"Missing ingredient: {ingredient.amount}x {ingredient.itemConfig.itemName} (min rank: {ingredient.minRank})");
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

        // Добавьте эту проверку
        if (recipe.resultItem == null)
        {
            Debug.LogError($"Recipe '{recipe.name}' has no result item assigned!");
            return;
        }
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient != null && ingredient.itemConfig != null)
            {
                inventory.CmdRemoveItem(
                    ingredient.itemConfig.name, 
                    ingredient.amount, 
                    ingredient.minRank
                );
            }
        }

        if (recipe.resultItem != null)
        {
            var itemData = new ItemData {
                configId = recipe.resultItem.name,
                rank = CalculateResultRank(recipe)
            };
            inventory.PutInEmptySlot(recipe.resultItem, itemData);
        }
    
        PlayCraftEffect();
    }

    private ItemRank CalculateResultRank(ItemRecipe recipe)
    {
        return ItemRank.C;
    }

    // private bool IsPlayerInRange()
    // {
    //     Collider[] hitColliders = Physics.OverlapSphere(transform.position, craftRadius);
    //     foreach (var hitCollider in hitColliders)
    //     {
    //         if (hitCollider != null && 
    //             hitCollider.CompareTag("Player") && 
    //             hitCollider.GetComponent<NetworkIdentity>()?.isLocalPlayer == true)
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
}