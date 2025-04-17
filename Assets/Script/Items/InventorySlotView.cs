using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour {
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private Button _button;

    [HideInInspector]
    public ItemConfig slotItemConfig;

    [HideInInspector]
    public ItemData slotItemData;

    private void Start() {
        InitButton();
    }

    private void InitButton() {
        _button.onClick.AddListener(ShowInfo);
    }

    public void PutInSlot(ItemConfig itemConfig, ItemData itemData) {
        _icon.sprite = itemConfig.icon;
        slotItemConfig = itemConfig;
        slotItemData = itemData;
        _icon.enabled = true;
    }

    void ShowInfo() {
        if (slotItemConfig != null) {
            ItemInfo.instance.Open(slotItemConfig, slotItemData, this);
        }
    }

    public void ClearSlot() {
        slotItemConfig = null;
        slotItemData = null;
        _icon.sprite = null;
        _icon.enabled = false;
    }
}