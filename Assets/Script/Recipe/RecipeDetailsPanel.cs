using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class RecipeDetailsPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image recipeIcon;
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private TMP_Text recipeDescription;
    [SerializeField] private TMP_Text ingredientsText;
    [SerializeField] private TMP_Text requiredLevelText;
    [SerializeField] private TMP_Text resultItemText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Slider craftProgress;
    
    private ItemRecipe currentRecipe;
    private Building currentBuilding;
    private bool isCrafting;
    
    private void Awake()
    {
        if (closeButton != null) 
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(HidePanel);
        }

        if (createButton != null)
        {
            createButton.onClick.RemoveAllListeners();
            createButton.onClick.AddListener(OnCreateButtonClicked);
            Debug.Log("Create button listener assigned", this);
        }
        else
        {
            Debug.LogError("CreateButton reference is missing!", this);
        }

        if (craftProgress != null) 
            craftProgress.gameObject.SetActive(false);
    }
    private void OnCreateButtonClicked()
    {
        Debug.Log("Create button clicked", this);
        StartCrafting();
    }
    public void ShowRecipe(ItemRecipe recipe, Building building)
    {
        if (recipe == null || building == null)
        {
            Debug.LogError("Recipe or Building is null");
            return;
        }

        currentRecipe = recipe;
        currentBuilding = building;

            if (recipeIcon != null) recipeIcon.sprite = recipe.recipeIcon;
        if (recipeName != null) recipeName.text = recipe.recipeName ?? "Unknown Recipe";
        if (recipeDescription != null) recipeDescription.text = recipe.description ?? string.Empty;
        if (ingredientsText != null) ingredientsText.text = GetIngredientsList(recipe);
        if (requiredLevelText != null) requiredLevelText.text = $"Required Level: {recipe.requiredBuildingLevel}";
        if (resultItemText != null) resultItemText.text = GetResultItemText(recipe);

        UpdateCraftButtonState();
        gameObject.SetActive(true);
    }

    
    
    private string GetIngredientsList(ItemRecipe recipe)
    {
        if (recipe?.ingredients == null) return "Ingredients: None";

        var sb = new StringBuilder("Ingredients:\n");
        foreach (var ingredient in recipe.ingredients)
        {
            if (ingredient?.itemConfig != null)
            {
                string rankReq = ingredient.minRank > ItemRank.D ? 
                    $" (min {ingredient.minRank})" : "";
                sb.AppendLine($"- {ingredient.amount}x {ingredient.itemConfig.itemName}{rankReq}");
            }
        }
        return sb.ToString();
    }

    private string GetResultItemText(ItemRecipe recipe)
    {
        if (recipe?.resultItem == null) return "Result: Unknown";
        return $"Result: {recipe.resultItem.itemName} (x{recipe.resultAmount})";
    }

    private void UpdateCraftButtonState()
    {
        if (createButton == null) return;

        bool isCraftingInProgress = isCrafting;
    
        bool hasValidRecipe = currentRecipe != null;
        bool hasValidBuilding = currentBuilding != null;
    
        var inventory = LocalPlayer.Instance?.GetComponent<PlayerInventory>();
        bool hasInventory = inventory != null;
    
        bool hasIngredients = !isCraftingInProgress && 
                              currentBuilding?.CanCraft(currentRecipe, inventory) == true;

        createButton.interactable = hasValidRecipe && 
                                    hasValidBuilding && 
                                    hasInventory && 
                                    !isCraftingInProgress && 
                                    hasIngredients;

        Debug.Log($"Button State Update: " +
                  $"Recipe={hasValidRecipe}, " +
                  $"Building={hasValidBuilding}, " +
                  $"Inventory={hasInventory}, " +
                  $"Crafting={isCraftingInProgress}, " +
                  $"Ingredients={hasIngredients}");
    }

    private void HidePanel() => gameObject.SetActive(false);

    private void StartCrafting()
    {
        Debug.Log($"StartCrafting called. isCrafting: {isCrafting}, recipe: {currentRecipe != null}");
        
        if (isCrafting || currentRecipe == null) 
        {
            Debug.LogWarning($"Crafting conditions not met. isCrafting: {isCrafting}, recipe: {currentRecipe != null}");
            return;
        }

        StartCoroutine(CraftingRoutine());
    }
    private System.Collections.IEnumerator CraftingRoutine()
    {
        isCrafting = true;
        UpdateButtonState();
        
        Debug.Log($"Starting craft: {currentRecipe.recipeName}");

        float progress = 0;
        if (craftProgress != null)
        {
            craftProgress.gameObject.SetActive(true);
            craftProgress.value = 0;
        }

        while (progress < currentRecipe.craftTime)
        {
            progress += Time.deltaTime;
            if (craftProgress != null)
                craftProgress.value = progress / currentRecipe.craftTime;
            yield return null;
        }

        FinishCrafting();
    }
    private void FinishCrafting()
    {
        Debug.Log($"Completing craft: {currentRecipe.recipeName}");
        
        var inventory = GetPlayerInventory();
        if (inventory != null && currentBuilding != null)
        {
            currentBuilding.CraftItem(currentRecipe, inventory);
        }

        isCrafting = false;
        if (craftProgress != null)
            craftProgress.gameObject.SetActive(false);
        
        UpdateButtonState();
    }
    private PlayerInventory GetPlayerInventory()
    {
        if (LocalPlayer.Instance == null)
        {
            Debug.LogError("LocalPlayer instance is null!");
            return null;
        }

        var inventory = LocalPlayer.Instance.GetComponent<PlayerInventory>();
        if (inventory == null)
            Debug.LogError("PlayerInventory component missing!");

        return inventory;
    }
    private void UpdateButtonState()
    {
        if (createButton == null) return;

        bool canCraft = CanCraftNow();
        createButton.interactable = canCraft;
        Debug.Log($"UpdateButtonState. CanCraft: {canCraft}");
    }
    private bool CanCraftNow()
    {
        if (isCrafting || currentRecipe == null || currentBuilding == null)
            return false;

        var inventory = GetPlayerInventory();
        if (inventory == null)
            return false;

        return currentBuilding.CanCraft(currentRecipe, inventory);
    }
    private System.Collections.IEnumerator CraftingProcess()
    {
        isCrafting = true;
        if (craftProgress != null) craftProgress.gameObject.SetActive(true);

        float timer = 0;
        while (timer < currentRecipe.craftTime)
        {
            timer += Time.deltaTime;
            if (craftProgress != null) craftProgress.value = timer / currentRecipe.craftTime;
            yield return null;
        }

        CompleteCrafting();
    }

    private void CompleteCrafting()
    {
        var inventory = LocalPlayer.Instance?.GetComponent<PlayerInventory>();
        currentBuilding?.CraftItem(currentRecipe, inventory);
        
        isCrafting = false;
        if (craftProgress != null) craftProgress.gameObject.SetActive(false);
        UpdateCraftButtonState();
    }
}