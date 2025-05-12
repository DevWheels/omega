using System;
using System.Collections.Generic;
using UnityEngine;

public class UnwearItemButton : MonoBehaviour{
    private List<SkillConfig> _skillConfig;
    private ItemBase _itemBase;
    private Action<ItemBase,SkillConfig> _onSelected;
    public void SetData(List<SkillConfig> skillConfig,ItemBase itemBase,Action<ItemBase,SkillConfig> onSelected) {
        _itemBase = itemBase;
        _onSelected = onSelected;
        _skillConfig = skillConfig;
    }

    // private void OnUnwearItem() {
    //     _onSelected?.Invoke(_itemBase,_skillConfig);
    // }
}
