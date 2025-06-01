using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecipeButtonController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image recipeIcon; 
    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private TMP_Text requiredLevelText;
    [SerializeField] private Button button;
    
    private ItemRecipe currentRecipe;
    private Building currentBuilding;
    private RecipeDetailsPanel detailsPanel;
    
    public void Initialize(ItemRecipe recipe, Building building, RecipeDetailsPanel detailsPanel)
    {
        currentRecipe = recipe;
        currentBuilding = building;
        this.detailsPanel = detailsPanel;
        
        if (recipeIcon != null) 
        {
            recipeIcon.sprite = recipe.recipeIcon;
            recipeIcon.preserveAspect = true;
        }
        
        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.recipeName;
        }
        
        if (requiredLevelText != null)
        {
            requiredLevelText.text = $"Требуется уровень: {recipe.requiredBuildingLevel}";
        }
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ShowRecipeDetails);
    }
    
    private void ShowRecipeDetails()
    {
        if (detailsPanel != null && currentRecipe != null)
        {
            detailsPanel.ShowRecipe(currentRecipe, currentBuilding);
        }
        else
        {
            Debug.LogError("Details panel or recipe is not set!", this);
        }
    }
    
    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}