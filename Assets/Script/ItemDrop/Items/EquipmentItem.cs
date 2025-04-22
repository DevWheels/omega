using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : EquipmentItemConfig
{
    public EquipmentItemConfig Config { get; private set; }
    public List<string> Skills { get; private set; }
    private PlayerEquipment playerEquipment;
    public ItemRank Rank { get; private set; }
    public int Level { get; private set; }
    public int Health { get; private set; }
    public int Armor { get; private set; }
    public int Attack { get; private set; }
    public SpecialStatType[] SpecialStats { get; private set; }
    public float[] SpecialStatsValues { get; private set; }
    public EquipmentItem(EquipmentItemConfig config, int mobLevel)
    {
        Config = config;
        ItemRank rank = DetermineRankByMobLevel(mobLevel);
        Generate(rank, mobLevel, mobLevel); 
    }
    private ItemRank DetermineRankByMobLevel(int mobLevel)
    {
   
        Dictionary<ItemRank, float> rankChances = GetRankChancesForMobLevel(mobLevel);


        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float cumulativeChance = 0f;


        foreach (var kvp in rankChances)
        {
            cumulativeChance += kvp.Value;
            if (randomValue <= cumulativeChance)
            {
                return kvp.Key;
            }
        }
        
        return ItemRank.D;
    }
    
    private Dictionary<ItemRank, float> GetRankChancesForMobLevel(int mobLevel)
    {
        var chances = new Dictionary<ItemRank, float>();
        if (mobLevel <= 5)
        {
            chances[ItemRank.D] = 100f;
        }
        else if (mobLevel <= 10)
        {
            chances[ItemRank.D] = 80f;
            chances[ItemRank.C] = 20f;
        }
        else if (mobLevel <= 15)
        {
            chances[ItemRank.D] = 60f;
            chances[ItemRank.C] = 35f;
            chances[ItemRank.B] = 5f;
        }
        else if (mobLevel <= 20)
        {
            chances[ItemRank.D] = 40f;
            chances[ItemRank.C] = 45f;
            chances[ItemRank.B] = 14f;
            chances[ItemRank.A] = 1f;
        }
        else if (mobLevel <= 25)
        {
            chances[ItemRank.D] = 30f;
            chances[ItemRank.C] = 40f;
            chances[ItemRank.B] = 25f;
            chances[ItemRank.A] = 4.5f;
            chances[ItemRank.S] = 0.5f;
        }
        else 
        {
            chances[ItemRank.D] = 20f;
            chances[ItemRank.C] = 30f;
            chances[ItemRank.B] = 35f;
            chances[ItemRank.A] = 12f;
            chances[ItemRank.S] = 3f;
        }

        return chances;
    }
    
    public void Generate(ItemRank rank, int playerLevel, int mobLevel)
    {
        Rank = rank;
        Level = playerLevel + Mathf.RoundToInt(mobLevel * 0.5f);
        
        var ranges = Config.GetRangesForRank(rank);
         int levelBonus = (Level - 1) * ranges.perLevelIncrease;
        
        Health = Random.Range(
            ranges.health.x + levelBonus,
            ranges.health.y + levelBonus + 50
        );
        
        Armor = Random.Range(
            ranges.armor.x + levelBonus,
            ranges.armor.y + levelBonus + 5
        );
        
        Attack = Random.Range(
            ranges.attack.x + levelBonus,
            ranges.attack.y + levelBonus + 5
        );
        Skills = SkillsTable.Instance.GetSkillsForItem();
        playerEquipment = FindAnyObjectByType<PlayerEquipment>();

        GenerateSpecialStats();
        LogStats();
    }
    
    private void GenerateSpecialStats()
    {
        int statsCount = Rank switch
        {
            ItemRank.D => 0,
            ItemRank.C or ItemRank.B => 1,
            ItemRank.A or ItemRank.S => 2,
            _ => 0
        };
        SpecialStats = new SpecialStatType[statsCount];
        SpecialStatsValues = new float[statsCount];
        for (int i = 0; i < statsCount; i++)
        {
            SpecialStats[i] = GetRandomSpecialStat();
            SpecialStatsValues[i] = CalculateSpecialStatValue(SpecialStats[i]);
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
        return baseValue * (1 + (Level * 0.05f));
    }
    public void LogStats()
    {
        string log = $"[{Rank}] {itemName} (Lvl {Level})\n" +
                    $"HP: {Health} | Armor: {Armor} | ATK: {Attack}\n";
        Debug.Log(log);
    }
}