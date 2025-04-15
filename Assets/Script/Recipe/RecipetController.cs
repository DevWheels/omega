using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipetController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private Button showRecipeButton;

    [Header("Debug")]
    [SerializeField] private ItemRecipe currentRecipe;
    private RecipeDetailsPanel detailsPanel;

    private void Awake()
    {
        // Автоматически находим кнопку, если не назначена вручную
        if (showRecipeButton == null)
        {
            showRecipeButton = GetComponentInChildren<Button>();
        }

        // Находим панель деталей (можно заменить на ссылку через инспектор)
        if (detailsPanel == null)
        {
            detailsPanel = FindObjectOfType<RecipeDetailsPanel>(true);
        }

        // Защита от null reference
        if (showRecipeButton != null)
        {
            showRecipeButton.onClick.AddListener(ShowRecipeDetails);
        }
        else
        {
            Debug.LogError("ShowRecipeButton not assigned!", this);
        }
    }

    public void Setup(ItemRecipe recipe)
    {
        currentRecipe = recipe;
        
        if (itemIcon != null)
        {
            itemIcon.sprite = recipe.itemIcon;
            itemIcon.preserveAspect = true;
        }

        if (itemDescription != null)
        {
            itemDescription.text = recipe.description;
        }
    }

    public void ShowRecipeDetails()
    {
        if (detailsPanel == null)
        {
            Debug.LogError("Details panel not found!", this);
            return;
        }

        if (currentRecipe == null)
        {
            Debug.LogError("Current recipe not set!", this);
            return;
        }

        detailsPanel.ShowRecipe(currentRecipe);
    }

    // Очистка listeners при уничтожении
    private void OnDestroy()
    {
        if (showRecipeButton != null)
        {
            showRecipeButton.onClick.RemoveAllListeners();
        }
    }
}