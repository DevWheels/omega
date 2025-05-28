using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipetController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Button showRecipeButton;

    private ItemRecipe currentRecipe;
    private Building currentBuilding;
    private RecipeDetailsPanel detailsPanel;

    private void Awake()
    {
        showRecipeButton = showRecipeButton ?? GetComponentInChildren<Button>();
        detailsPanel = detailsPanel ?? FindObjectOfType<RecipeDetailsPanel>(true);

        if (showRecipeButton != null)
        {
            showRecipeButton.onClick.AddListener(ShowRecipeDetails);
        }
        else
        {
            Debug.LogError("ShowRecipeButton not found!", this);
        }
    }

    public void Setup(ItemRecipe recipe, Building building)
    {
        currentRecipe = recipe;
        currentBuilding = building;

        if (itemIcon != null && recipe?.recipeIcon != null)
        {
            itemIcon.sprite = recipe.recipeIcon;
            itemIcon.preserveAspect = true;
        }

        if (itemDescription != null)
        {
            itemDescription.text = recipe?.description ?? "No description available";
        }
    }

    private void ShowRecipeDetails()
    {
        if (detailsPanel == null)
        {
            Debug.LogError("Details panel reference is null!", this);
            return;
        }

        if (currentRecipe == null || currentBuilding == null)
        {
            Debug.LogError("Recipe or Building reference is null!", this);
            return;
        }

        detailsPanel.ShowRecipe(currentRecipe, currentBuilding);
    }

    private void OnDestroy()
    {
        if (showRecipeButton != null)
        {
            showRecipeButton.onClick.RemoveListener(ShowRecipeDetails);
        }
    }
}