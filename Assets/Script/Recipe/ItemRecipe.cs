using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class ItemRecipe : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string description;
    public Ingredient[] ingredients; // Ингредиенты для крафта
}

[System.Serializable]
public class Ingredient
{
    public ItemRecipe item; // Какой предмет нужен
    public int amount;      // Сколько нужно
}