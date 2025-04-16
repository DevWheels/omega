using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : EquipmentItemConfigConfig
{
    public EquipmentItemConfigConfig ConfigConfig { get; private set; }
    
    public ItemRank Rank { get; private set; }
    public int Level { get; private set; }
    public int Health { get; private set; }
    public int Armor { get; private set; }
    public int Attack { get; private set; }
    public SpecialStatType[] SpecialStats { get; private set; }
    public float[] SpecialStatsValues { get; private set; }
    public EquipmentItem(EquipmentItemConfigConfig configConfig, ItemRank rank, int playerLevel, int mobLevel)
    {
        ConfigConfig = configConfig;
        Generate(rank, playerLevel, mobLevel);
        
    }
    public void Generate(ItemRank rank, int playerLevel, int mobLevel)
    {
        Rank = rank;
        Level = playerLevel + Mathf.RoundToInt(mobLevel * 0.5f);
        
        var ranges = ConfigConfig.GetRangesForRank(rank);
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