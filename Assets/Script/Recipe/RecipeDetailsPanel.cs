using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class RecipeDetailsPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image recipeIcon;
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private TMP_Text recipeDescription;
    [SerializeField] private Transform ingredientsContainer; 
    [SerializeField] private GameObject ingredientIconPrefab; 
    [SerializeField] private TMP_Text requiredLevelText;
    [SerializeField] private TMP_Text resultItemText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createButton;
    [SerializeField] private Slider craftProgress;
    [SerializeField] private Slider craftProgressSlider;
    private ItemRecipe currentRecipe;
    private Building currentBuilding;
    private bool isCrafting;
    [SerializeField] private ItemRecipe[] allRecipes;

    void Start()
    {
        Debug.Log($"Checking recipes count: {allRecipes.Length}");
    

    
        if (allRecipes != null && allRecipes.Length > 0)
        {
            var testRecipe = RecipeDatabase.GetRecipeById(allRecipes[0].name);
        }
    
        foreach (var recipe in allRecipes)
        {
            if (recipe != null && recipe.name == "Книга Альбуса Дамблдора")
            {
                Debug.Log($"Found target recipe: {recipe.name}");
            }
        }
    }
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
        if (requiredLevelText != null) requiredLevelText.text = $"Required Level: {recipe.requiredBuildingLevel}";
        if (resultItemText != null) resultItemText.text = GetResultItemText(recipe);

        ClearIngredientIcons();

        if (ingredientsContainer != null && ingredientIconPrefab != null)
        {
            foreach (var ingredient in recipe.ingredients.Where(i => i != null && i.itemConfig != null))
            {
                var iconGO = Instantiate(ingredientIconPrefab, ingredientsContainer);
                var iconImage = iconGO.GetComponentInChildren<Image>();
                var amountText = iconGO.GetComponentInChildren<TMP_Text>();

                if (iconImage != null)
                {
                    iconImage.sprite = ingredient.itemConfig.icon;
                    iconImage.preserveAspect = true;
                }

                if (amountText != null)
                {
                    amountText.text = ingredient.amount.ToString();
                }
            }
        }

        UpdateCraftButtonState();
        gameObject.SetActive(true);
    }

    private void ClearIngredientIcons()
    {
        if (ingredientsContainer == null) return;

        foreach (Transform child in ingredientsContainer)
        {
            Destroy(child.gameObject);
        }
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
    
        bool hasIngredients = false;
    
        if (!isCraftingInProgress && hasValidRecipe && hasValidBuilding && hasInventory)
        {
            hasIngredients = currentBuilding.CanCraft(currentRecipe, inventory);
        
            // Добавляем подробное логирование
            if (!hasIngredients)
            {
                Debug.Log("Missing ingredients:");
                foreach (var ingredient in currentRecipe.ingredients)
                {
                    if (ingredient != null && ingredient.itemConfig != null)
                    {
                        bool has = inventory.HasItem(ingredient.itemConfig, ingredient.amount, ingredient.minRank);
                        Debug.Log($"- {ingredient.itemConfig.itemName}: {ingredient.amount} (min rank {ingredient.minRank}) - {(has ? "OK" : "MISSING")}");
                    }
                }
            }
        }

        createButton.interactable = hasValidRecipe && 
                                    hasValidBuilding && 
                                    hasInventory && 
                                    !isCraftingInProgress && 
                                    hasIngredients;
    }

    private void HidePanel()
    {
        craftProgressSlider.gameObject.SetActive(false);
        craftProgressSlider.value = 0f;
        gameObject.SetActive(false);
    }

    private void StartCrafting()
    {
        if (isCrafting || currentRecipe == null) 
        {
            Debug.LogWarning("Crafting already in progress or recipe is null!");
            return;
        }

        StartCoroutine(CraftingRoutine());
    }

    private System.Collections.IEnumerator CraftingRoutine()
    {
        isCrafting = true;
        UpdateButtonState();

        float progress = 0f;
        craftProgressSlider.gameObject.SetActive(true);
        craftProgressSlider.value = 0f;

        while (progress < currentRecipe.craftTime)
        {
            progress += Time.deltaTime;
            float normalizedProgress = Mathf.Clamp01(progress / currentRecipe.craftTime);
            craftProgressSlider.value = normalizedProgress;
            yield return null;
        }


        craftProgressSlider.value = 1f;
        yield return new WaitForSeconds(0.1f); 
        FinishCrafting();
    }

    private void FinishCrafting()
    {
        Debug.Log($"Attempting to craft: {currentRecipe?.name}");
    
        if (currentRecipe == null)
        {
            Debug.LogError("Current recipe is null!");
            return;
        }

        var inventory = LocalPlayer.Instance?.GetComponent<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory is null!");
            return;
        }

        Debug.Log($"Передаваемое имя рецепта: '{currentRecipe.name}'");
        inventory.CmdCraftItem(currentRecipe.name);
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
    
    private void OnEnable()
    {
        if (LocalPlayer.Instance != null)
        {
            var inventory = LocalPlayer.Instance.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.OnInventoryChanged += UpdateCraftButtonState;
            }
        }
        UpdateCraftButtonState();
    }

    private void OnDisable()
    {
        if (LocalPlayer.Instance != null)
        {
            var inventory = LocalPlayer.Instance.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= UpdateCraftButtonState;
            }
        }
    }
    
}