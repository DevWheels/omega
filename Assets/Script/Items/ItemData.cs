using System;
using System.Collections.Generic;

[Serializable]
public class ItemData {
    public PickItemType Type;
    public List<SkillType> Skills = new List<SkillType>();
    public string configId;
    public ItemRank rank;
    public bool IsUsable => Type==PickItemType.Apple || Type==PickItemType.ManaPotion;
    
}

public enum PickItemType {
    Apple,ManaPotion,Boots,Staff, Chestplate,ingredients
}
