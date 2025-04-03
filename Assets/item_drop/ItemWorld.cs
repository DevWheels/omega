using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class ItemWorld : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private ItemTooltip tooltip;

    [Header("Debug")]
    [SerializeField] private bool autoFindTooltip = true;

    private Iteme item;

    private void Awake()
    {
        if (tooltip == null && autoFindTooltip)
        {
            tooltip = FindObjectOfType<ItemTooltip>();
            if (tooltip == null)
            {
                Debug.LogWarning($"{name}: ItemTooltip not found in scene");
            }
        }
    }

    public void SetItem(Iteme newItem)
    {
        item = newItem;

    }
    public Iteme GetItem()
    {
        return item;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null && item != null)
        {
            tooltip.ShowTooltip(item);
        }
        else
        {
            Debug.LogWarning($"Cannot show tooltip: tooltip={tooltip}, item={item}");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
    }
}