using UnityEngine;

public class RecipeListPopulator : MonoBehaviour
{
    [Header("Settings")]
    public GameObject recipetPrefab;
    public ItemRecipe[] allRecipes;
    
    [Header("Dependencies")]
    [SerializeField] private Building currentBuilding;

    private void Start()
    {
        if (recipetPrefab == null)
        {
            Debug.LogError("Recipe prefab is not assigned!", this);
            return;
        }

        if (recipetPrefab.GetComponent<RecipetController>() == null)
        {
            Debug.LogError("Prefab is missing RecipetController component!", this);
            return;
        }

        if (currentBuilding == null)
        {
            currentBuilding = FindObjectOfType<Building>();
            if (currentBuilding == null)
            {
                Debug.LogError("No Building found in scene!", this);
                return;
            }
        }

        PopulateRecipes();
    }

    public void PopulateRecipes()
    {
        ClearExistingRecipes();

        if (allRecipes == null || allRecipes.Length == 0)
        {
            Debug.LogWarning("No recipes assigned to populate", this);
            return;
        }

        foreach (var recipe in allRecipes)
        {
            if (recipe == null) continue;

            var newRecipet = Instantiate(recipetPrefab, transform);
            var controller = newRecipet.GetComponent<RecipetController>();
            
            if (controller != null)
            {
                controller.Setup(recipe, currentBuilding);
            }
        }
    }

    private void ClearExistingRecipes()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetBuilding(Building building)
    {
        currentBuilding = building;
        PopulateRecipes();
    }
}