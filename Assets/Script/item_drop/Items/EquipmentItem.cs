using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public GameObject worldPrefab;

    [Header("Stat Ranges By Rank")]
    public RankStatRanges dRankStats;
    public RankStatRanges cRankStats;
    public RankStatRanges bRankStats;
    public RankStatRanges aRankStats;
    public RankStatRanges sRankStats;

    [Header("Generated Stats")]
    public ItemRank rank;
    public int level;
    public int health;
    public int armor;
    public int attack;
    public SpecialStatType[] specialStats;
    public float[] specialStatsValues;

    public void Generate(ItemRank itemRank, int playerLevel, int mobLevel)
    {
        rank = itemRank;
        level = playerLevel + Mathf.RoundToInt(mobLevel * 0.5f); 

        RankStatRanges ranges = GetRangesForRank(rank);
        int levelBonus = (level - 1) * ranges.perLevelIncrease;

        health = Random.Range(
            ranges.health.x + levelBonus,
            ranges.health.y + levelBonus + 1
        );

        armor = Random.Range(
            ranges.armor.x + levelBonus,
            ranges.armor.y + levelBonus + 1
        );

        attack = Random.Range(
            ranges.attack.x + levelBonus,
            ranges.attack.y + levelBonus + 1
        );

        GenerateSpecialStats();
        LogStats();
    }

    private RankStatRanges GetRangesForRank(ItemRank rank)
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

    private void GenerateSpecialStats()
    {
        int statsCount = rank switch
        {
            ItemRank.D => 0,
            ItemRank.C or ItemRank.B => 1,
            ItemRank.A or ItemRank.S => 2,
            _ => 0
        };

        specialStats = new SpecialStatType[statsCount];
        specialStatsValues = new float[statsCount];

        for (int i = 0; i < statsCount; i++)
        {
            specialStats[i] = GetRandomSpecialStat();
            specialStatsValues[i] = CalculateSpecialStatValue(specialStats[i]);
        }
    }

    private SpecialStatType GetRandomSpecialStat()
    {
        Array values = Enum.GetValues(typeof(SpecialStatType));
        return (SpecialStatType)values.GetValue(Random.Range(0, values.Length));
    }

    private float CalculateSpecialStatValue(SpecialStatType stat)
    {
        float baseValue = stat switch
        {
            SpecialStatType.Dodge => Random.Range(1f, 3f),
            SpecialStatType.CRIT => Random.Range(2f, 5f),

            _ => 0f
        };

        return baseValue * (1 + (level * 0.05f));
    }

    public void LogStats()
    {
        string log = $"[{rank}] {itemName} (Lvl {level})\n" +
                    $"HP: {health} | Armor: {armor} | ATK: {attack}\n";

        if (specialStats.Length > 0)
        {
            log += "Special Stats:";
            for (int i = 0; i < specialStats.Length; i++)
            {
                log += $"\n- {specialStats[i]}: {specialStatsValues[i]:F1}";
            }
        }

        Debug.Log(log);
    }
}