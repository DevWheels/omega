using UnityEngine;
using static UnityEditor.Progress;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ItemDropSystem itemDropSystem;
    private int playerLevel = 1;

    public void Defeated()
    {
        Debug.Log($"Enemy defeated at {transform.position}");
        GameObject droppedItem = itemDropSystem.TryDropItem(playerLevel, transform.position);

        if (droppedItem != null)
        {
            Debug.Log($"Item dropped: {droppedItem.name}");
        }
        else
        {
            Debug.Log("No item dropped");
        }

        Destroy(gameObject);
    }
}
