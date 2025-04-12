using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDetailsPanel : MonoBehaviour
{
    public Image recipeIcon;
    public TMP_Text recipeName;
    public TMP_Text recipeDescription;
    public TMP_Text ingredientsText;
    public Button closeButton;
    public Button createButton;

    private void Start()
    {
        closeButton.onClick.AddListener(Hide);
        createButton.onClick.AddListener(CraftItem);
        gameObject.SetActive(false);
    }

    public void ShowRecipe(ItemRecipe recipe)
    {
        recipeIcon.sprite = recipe.itemIcon;
        recipeName.text = recipe.itemName;
        recipeDescription.text = recipe.description;
        ingredientsText.text = GetIngredientsList(recipe);

        gameObject.SetActive(true); // Показываем панель
    }

    private string GetIngredientsList(ItemRecipe recipe)
    {
        string result = "Необходимо:\n";
        foreach (var ingredient in recipe.ingredients)
        {
            result += $"- {ingredient.amount}x {ingredient.item.itemName}\n";
        }
        return result;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void CraftItem()
    {
        // Логика крафта
        Debug.Log("Создан предмет: ");
    }
}