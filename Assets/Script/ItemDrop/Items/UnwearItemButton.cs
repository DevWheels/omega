using System;
using System.Collections.Generic;
using UnityEngine;

public class UnwearItemButton : MonoBehaviour{
    private List<SkillConfig> _skillConfig;
    private ItemConfig _itemConfig;
    private ItemData _itemData;
    private Action<ItemConfig,ItemData,List<SkillConfig>> _onSelected;
    
    public void SetData(List<SkillConfig> skillConfig,ItemConfig itemConfig,ItemData data,Action<ItemConfig,ItemData,List<SkillConfig>> onSelected) {
        _itemConfig = itemConfig;
        _skillConfig = skillConfig;
        _onSelected = onSelected;
        _skillConfig = skillConfig;
    }
    public void Disable() {
        GameUI.Instance.button.SetActive(false);
    }

    public void Enable() {
        GameUI.Instance.button.SetActive(true);
        
    }
    private void OnUnwearItem() {
        _onSelected?.Invoke(_itemConfig,_itemData,_skillConfig);
        Disable();
    }
}
