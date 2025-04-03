using System.Collections.Generic;
using System;
using UnityEngine;
using static ItemSettings;
using static UnityEditor.Progress;
using Random = UnityEngine.Random;


public class ItemFactory : MonoBehaviour
{
    [SerializeField] private ItemSettings itemSettings;
    [SerializeField] private GameObject itemPrefab;
    private System.Random random = new System.Random();

    public GameObject CreateRandomItemObject(int playerLevel, Vector3 spawnPosition)
    {
        Iteme item = CreateRandomItem(playerLevel);
        GameObject itemObj = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        itemObj.GetComponent<ItemWorld>().SetItem(item);
        return itemObj;
    }

    public Iteme CreateRandomItem(int playerLevel)
    {
        ItemRank rank = DetermineItemRank();
        int itemLevel = playerLevel + 2;

        RankStats stats = GetRankStats(rank);

        int minHealth = stats.baseHealthRange.x + (stats.healthPerLevel * itemLevel);
        int maxHealth = stats.baseHealthRange.y + (stats.healthPerLevel * itemLevel);
        int health = Random.Range(minHealth, maxHealth + 1);

        int minArmor = stats.baseArmorRange.x + (stats.armorPerLevel * itemLevel);
        int maxArmor = stats.baseArmorRange.y + (stats.armorPerLevel * itemLevel);
        int armor = Random.Range(minArmor, maxArmor + 1);

        int minAttack = stats.baseAttackRange.x + (stats.attackPerLevel * itemLevel);
        int maxAttack = stats.baseAttackRange.y + (stats.attackPerLevel * itemLevel);
        int attack = Random.Range(minAttack, maxAttack + 1);

        List<SpecialStat> specialStats = GenerateSpecialStats(rank);
        string itemName = GenerateItemName(rank, itemLevel);

        Debug.Log($"Created {rank} item for player lvl {playerLevel} (item lvl {itemLevel})");
        return new Iteme(itemName, rank, itemLevel, health, armor, attack, specialStats);
    }

    public GameObject CreateSpecificRankItemObject(ItemRank rank, int playerLevel, Vector3 spawnPosition)
    {
        Iteme item = CreateSpecificRankItem(rank, playerLevel);
        GameObject itemObj = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        itemObj.GetComponent<ItemWorld>().SetItem(item);
        return itemObj;
    }

    public Iteme CreateSpecificRankItem(ItemRank rank, int playerLevel)
    {
        int itemLevel = playerLevel + 2;
        RankStats stats = GetRankStats(rank);

        int health = Random.Range(
            stats.baseHealthRange.x + (stats.healthPerLevel * itemLevel),
            stats.baseHealthRange.y + (stats.healthPerLevel * itemLevel) + 1);

        int armor = Random.Range(
            stats.baseArmorRange.x + (stats.armorPerLevel * itemLevel),
            stats.baseArmorRange.y + (stats.armorPerLevel * itemLevel) + 1);

        int attack = Random.Range(
            stats.baseAttackRange.x + (stats.attackPerLevel * itemLevel),
            stats.baseAttackRange.y + (stats.attackPerLevel * itemLevel) + 1);

        List<SpecialStat> specialStats = GenerateSpecialStats(rank);
        string itemName = GenerateItemName(rank, itemLevel);

        return new Iteme(itemName, rank, itemLevel, health, armor, attack, specialStats);
    }

    private ItemRank DetermineItemRank()
    {
        float roll = (float)random.NextDouble() * 100f;

        if (roll < itemSettings.dropChances.RankSChance) return ItemRank.S;
        roll -= itemSettings.dropChances.RankSChance;

        if (roll < itemSettings.dropChances.RankAChance) return ItemRank.A;
        roll -= itemSettings.dropChances.RankAChance;

        if (roll < itemSettings.dropChances.RankBChance) return ItemRank.B;
        roll -= itemSettings.dropChances.RankBChance;

        if (roll < itemSettings.dropChances.RankCChance) return ItemRank.C;

        return ItemRank.D;
    }

    private List<SpecialStat> GenerateSpecialStats(ItemRank rank)
    {
        List<SpecialStat> specialStats = new List<SpecialStat>();
        Array allStats = Enum.GetValues(typeof(SpecialStat));

        int statsCount = rank switch
        {
            ItemRank.S or ItemRank.A => 2,
            ItemRank.B or ItemRank.C => 1,
            _ => 0
        };

        while (specialStats.Count < statsCount)
        {
            SpecialStat randomStat = (SpecialStat)allStats.GetValue(random.Next(allStats.Length));
            if (!specialStats.Contains(randomStat))
                specialStats.Add(randomStat);
        }

        return specialStats;
    }

    private string GenerateItemName(ItemRank rank, int level)
    {
        string[] prefixes = rank switch
        {
            ItemRank.S => new[] { "Legendary", "Divine", "Exalted" },
            ItemRank.A => new[] { "Epic", "Mighty", "Supreme" },
            ItemRank.B => new[] { "Rare", "Strong", "Superior" },
            ItemRank.C => new[] { "Common", "Sturdy", "Reinforced" },
            _ => new[] { "Crude", "Weak", "Simple" }
        };

        string[] suffixes = { "Sword", "Shield", "Armor", "Amulet", "Ring" };

        return $"{prefixes[random.Next(prefixes.Length)]} {suffixes[random.Next(suffixes.Length)]}";
    }

    private RankStats GetRankStats(ItemRank rank) => rank switch
    {
        ItemRank.D => itemSettings.statsByRank.D,
        ItemRank.C => itemSettings.statsByRank.C,
        ItemRank.B => itemSettings.statsByRank.B,
        ItemRank.A => itemSettings.statsByRank.A,
        ItemRank.S => itemSettings.statsByRank.S,
        _ => itemSettings.statsByRank.D
    };
}
