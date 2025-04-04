using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LootSystem", menuName = "Loot/LootSystem")]
public class EnemyLootSystem : ScriptableObject
{
    [System.Serializable]
    public class DropConfig
    {
        public ItemRank rank;
        public float commonWeight;
        public float eliteWeight;
        public float championWeight;
        public float bossWeight;
        public float legendaryWeight;
    }

    public EquipmentItem[] possibleDrops;
    public DropConfig[] dropConfigs;

    public EquipmentItem GenerateDrop(EnemyRank enemyRank, int playerLevel, int mobLevel)
    {
        ItemRank itemRank = CalculateItemRank(enemyRank);
        EquipmentItem template = GetRandomItemOfRank(itemRank);

        if (template == null) return null;

        EquipmentItem item = Instantiate(template);
        item.Generate(itemRank, playerLevel, mobLevel);
        return item;
    }

    private ItemRank CalculateItemRank(EnemyRank enemyRank)
    {
        float totalWeight = dropConfigs.Sum(c => GetWeight(c, enemyRank));
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var config in dropConfigs)
        {
            currentWeight += GetWeight(config, enemyRank);
            if (randomValue <= currentWeight)
                return config.rank;
        }

        return ItemRank.D;
    }

    private float GetWeight(DropConfig config, EnemyRank enemyRank)
    {
        return enemyRank switch
        {
            EnemyRank.Common => config.commonWeight,
            EnemyRank.Elite => config.eliteWeight,
            EnemyRank.Champion => config.championWeight,
            EnemyRank.Boss => config.bossWeight,
            EnemyRank.Legendary => config.legendaryWeight,
            _ => 0f
        };
    }

    private EquipmentItem GetRandomItemOfRank(ItemRank rank)
    {
        var validItems = possibleDrops.Where(i => i != null).ToArray();
        if (validItems.Length == 0) return null;
        return validItems[Random.Range(0, validItems.Length)];
    }
}