using System;
using System.Collections.Generic;
using UnityEngine;

public class UnwearItemButton : MonoBehaviour{
    private ItemConfig _itemConfig;
    private ItemData _itemData;
    private Action<ItemConfig,ItemData> _onSelected;
    
    public void SetData(ItemConfig itemConfig,ItemData data,Action<ItemConfig,ItemData> onSelected) {
        _itemConfig = itemConfig;
        _itemData = data;
        _onSelected = onSelected;

    }
    public void Disable() {
        gameObject.SetActive(false);
    }

    public void Enable() {
        gameObject.SetActive(true);
        GameUI.Instance.button.transform.SetParent(transform);
        
    }
    public void OnUnwearItem() {
        _onSelected?.Invoke(_itemConfig,_itemData);
        Disable();
    }
}
