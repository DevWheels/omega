using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLootTable", menuName = "Loot/EnemyLootTable")]
public class EnemyLootTable : ScriptableObject
{
    [System.Serializable]
    public class RankDropSettings
    {
        public ItemRank rank;
        [Range(0f, 100f)] public float baseChance;
        public float commonModifier = 0.5f;
        public float eliteModifier = 1f;
        public float championModifier = 1.5f;
        public float bossModifier = 2f;
        public float legendaryModifier = 3f;
    }

    public EquipmentItem[] possibleItems;
    public RankDropSettings[] rankChances;

    public EquipmentItem GetRandomItem(EnemyRank enemyRank, int playerLevel, int mobLevel)
    {
        ItemRank itemRank = DetermineItemRank(enemyRank);
        EquipmentItem item = GetRandomItemOfRank(itemRank);

        if (item != null)
        {
            EquipmentItem droppedItem = Instantiate(item);
            droppedItem.Generate(itemRank, playerLevel, mobLevel); 
            return droppedItem;
        }

        return null;
    }

    private ItemRank DetermineItemRank(EnemyRank enemyRank)
    {
        float totalWeight = rankChances.Sum(setting => GetModifiedChance(setting, enemyRank));
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var setting in rankChances)
        {
            currentWeight += GetModifiedChance(setting, enemyRank);
            if (randomValue <= currentWeight)
            {
                return setting.rank;
            }
        }

        return ItemRank.D;
    }

    private float GetModifiedChance(RankDropSettings setting, EnemyRank enemyRank)
    {
        return setting.baseChance * (enemyRank switch
        {
            EnemyRank.Common => setting.commonModifier,
            EnemyRank.Elite => setting.eliteModifier,
            EnemyRank.Champion => setting.championModifier,
            EnemyRank.Boss => setting.bossModifier,
            EnemyRank.Legendary => setting.legendaryModifier,
            _ => 1f
        });
    }

    private EquipmentItem GetRandomItemOfRank(ItemRank rank)
    {
        var availableItems = possibleItems.Where(item => item != null).ToArray();
        return availableItems.Length == 0 ? null : availableItems[Random.Range(0, availableItems.Length)];
    }
}