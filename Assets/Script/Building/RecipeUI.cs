using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
public class RecipeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text recipeNameText;
    [SerializeField] private TMP_Text ingredientsText;
    [SerializeField] private Button craftButton;

    private AlchemyRecipe recipe;

    public void Initialize(AlchemyRecipe recipe)
    {
        this.recipe = recipe;
        recipeNameText.text = recipe.DisplayName;

        ingredientsText.text = "Ingredients:\n";
        foreach (var ingredient in recipe.Ingredients)
        {
            ingredientsText.text += $"- {ingredient.ItemType} x{ingredient.Amount}\n";
        }

        craftButton.onClick.AddListener(OnCraftButtonClick);
    }

    private void OnCraftButtonClick()
    {
        var player = NetworkClient.localPlayer.GetComponent<AlchemyBuilding>();
        player.CmdCraftPotion(recipe.RecipeId);
    }
}