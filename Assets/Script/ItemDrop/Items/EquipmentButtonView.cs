using System;

using UnityEngine;

public class EquipmentButtonView : MonoBehaviour{
    [SerializeField] private ItemType _itemType;
    private void ShowButton() {
        GameUI.Instance.button.SetData(PlayerEquipment.Instance.GetItemConfig(_itemType),PlayerEquipment.Instance.GetItemData(_itemType),PlayerEquipment.Instance.Unwear);
    }

    public void OnClick() {
        ShowButton();
    }
        
}
