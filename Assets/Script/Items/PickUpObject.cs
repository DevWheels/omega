using UnityEngine;
[RequireComponent(typeof(ItemBase))]
public class PickUpObject : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            var itemBase = GetComponent<ItemBase>();
            var inventory = collision.gameObject.GetComponent<PlayerInventory>();
            inventory.PutInEmptySlot(itemBase.itemConfig,itemBase.itemData);
            Destroy(gameObject);
        }
    }
}
