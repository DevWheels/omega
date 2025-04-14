using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentConfig", menuName = "Items/Equipment Config")]
public class EquipmentItemConfig : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public GameObject Prefab;
    [Header("Equipment Config")]
    public List<SkillConfig> itemSkills;
    [Header("Stat Ranges By Rank")]
    public RankStatRanges dRankStats;
    public RankStatRanges cRankStats;
    public RankStatRanges bRankStats;
    public RankStatRanges aRankStats;
    public RankStatRanges sRankStats;
    
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

    public class ItemSkills {
        [SerializeField] Skill skills;
    }
}