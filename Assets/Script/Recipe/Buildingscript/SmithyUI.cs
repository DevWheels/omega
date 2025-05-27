using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SmithyUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text buildingLevelText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Transform recipesContainer;
    [SerializeField] private GameObject recipeButtonPrefab;
    [SerializeField] private RecipeDetailsPanel recipeDetailsPanel;

    private Building currentBuilding;
    private ItemRecipe[] availableRecipes;

    public void Initialize(Building building, ItemRecipe[] recipes)
    {
        currentBuilding = building;
        availableRecipes = recipes;
        
        buildingNameText.text = building.BuildingName;
        buildingLevelText.text = $"Level: {building.buildingLevel}";
        
        closeButton.onClick.AddListener(() => building.CloseUI());
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        
        PopulateRecipes();
    }
    
    private void PopulateRecipes()
    {
        foreach (Transform child in recipesContainer)
        {
            Destroy(child.gameObject);
        }
        
        var sortedRecipes = availableRecipes.OrderBy(r => r.requiredBuildingLevel).ToArray();
        
        foreach (var recipe in sortedRecipes)
        {
            var recipeButton = Instantiate(recipeButtonPrefab, recipesContainer);
            var controller = recipeButton.GetComponent<RecipeButtonController>();
            controller.Initialize(recipe, currentBuilding, recipeDetailsPanel);
        }
    }
    
    private void OnUpgradeClicked()
    {
        currentBuilding.CmdUpgradeBuilding();
    }
}