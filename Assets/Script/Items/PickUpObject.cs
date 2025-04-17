using UnityEngine;
[RequireComponent(typeof(ItemBase))]
public class PickUpObject : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            var itemBase = GetComponent<ItemBase>();
            InventoryView.instance.PutInEmptySlot(itemBase.itemConfig,itemBase.itemData);
            InventoryView.instance.Player = collision.gameObject;
            Destroy(gameObject);
        }
    }
}
