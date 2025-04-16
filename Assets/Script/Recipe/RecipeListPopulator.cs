using UnityEngine;

public class RecipeListPopulator : MonoBehaviour
{
    public GameObject recipetPrefab; // Префаб Recipet
    public ItemRecipe[] allRecipes; // Все рецепты в игре

    private void Start()
    {
        foreach (var recipe in allRecipes)
        {
            GameObject newRecipet = Instantiate(recipetPrefab, transform);
            var controller = newRecipet.GetComponent<RecipetController>();
            controller.Setup(recipe);
        }
    }
}