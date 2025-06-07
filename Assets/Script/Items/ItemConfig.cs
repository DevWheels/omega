using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class ItemConfig : ScriptableObject
{

    [Header("������� ��������������")]
    // public string Name = "";
    public string Description = "�������� ��������";
    public Sprite icon = null;

    public bool isHealing;
    public float HealingPower;

    public bool isMana;
    public float ManaPower;

    [Header("������� ��������������")]
    public int time;

    [Header("�������������� ��� ��������")]
    public int Coins;
    public bool canBeStolen = true;
    public string PlayerPrefsName;
    

    
    [Header("Basic Settings")]
    public string itemName;
    public ItemType itemType;
    public ItemRank Rank;
    public GameObject Prefab;
    
    [Header("Skills")]
    public List<SkillType> Skills;
    
    [Header("Stat Ranges By Rank")]
    public RankStatRanges dRankStats;
    public RankStatRanges cRankStats;
    public RankStatRanges bRankStats;
    public RankStatRanges aRankStats;
    public RankStatRanges sRankStats;
    
    // [Header("Visuals")]
    // public Color itemColor = Color.white;

    public RankStatRanges GetRangesForRank(ItemRank rank)
    {
        return rank switch
        {
            ItemRank.D => dRankStats,
            ItemRank.C => cRankStats,
            ItemRank.B => bRankStats,
            ItemRank.A => aRankStats,
            ItemRank.S => sRankStats,
            _ => dRankStats
        };
    }

    [System.Serializable]
    public class RankStatRanges
    {
        public Vector2Int health;
        public Vector2Int attack;
        public Vector2Int armor;
        public int perLevelIncrease;
        public SpecialStatType[] possibleSpecialStats;
        public Vector2[] specialStatRanges;
    }
    
    public ItemRarity rarity;
}
public enum ItemRarity {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}