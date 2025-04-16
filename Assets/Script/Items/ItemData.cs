using System;

[Serializable]
public class ItemData {
    public PickItemType Type;
    public bool IsUsable => Type==PickItemType.Apple || Type==PickItemType.ManaPotion;
    
}

public enum PickItemType {
    Apple,ManaPotion,Boots,Staff
}
