using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class ItemRecipe : ScriptableObject
{
    public string recipeName;
    public Sprite recipeIcon;
    [TextArea] public string description;
    public Ingredient[] ingredients;
    public int requiredBuildingLevel = 1;
    public ItemConfig resultItem;
    public int resultAmount = 1;
    public float craftTime = 3f;
}

[System.Serializable]
public class Ingredient
{
    public ItemConfig itemConfig;
    public int amount;
    [Tooltip("Минимальный ранг требуемого предмета")]
    public ItemRank minRank = ItemRank.D;
}