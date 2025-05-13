// using System.Collections.Generic;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "NewEquipmentConfig", menuName = "Items/Equipment Config")]
// public class EquipmentItemConfig : ItemConfig
// {
//     [Header("Basic Settings")]
//     public string itemName;
//     public ItemType itemType;
//     public ItemRank Rank;
//     public GameObject Prefab;
//     
//     [Header("Skills")]
//     public List<SkillConfig> itemSkills;
//     
//     [Header("Stat Ranges By Rank")]
//     public RankStatRanges dRankStats;
//     public RankStatRanges cRankStats;
//     public RankStatRanges bRankStats;
//     public RankStatRanges aRankStats;
//     public RankStatRanges sRankStats;
//     
//     [Header("Visuals")]
//     public Sprite icon;
//     public Color itemColor = Color.white;
//     
//     //[Header("Scaling")]
//     // public int minLevel = 1;
//     // public int maxLevel = 30;
//
//     public RankStatRanges GetRangesForRank(ItemRank rank)
//     {
//         return rank switch
//         {
//             ItemRank.D => dRankStats,
//             ItemRank.C => cRankStats,
//             ItemRank.B => bRankStats,
//             ItemRank.A => aRankStats,
//             ItemRank.S => sRankStats,
//             _ => dRankStats
//         };
//     }
//
//     [System.Serializable]
//     public class RankStatRanges
//     {
//         public Vector2Int health;
//         public Vector2Int attack;
//         public Vector2Int armor;
//         public int perLevelIncrease;
//         public SpecialStatType[] possibleSpecialStats;
//         public Vector2[] specialStatRanges;
//     }
// }