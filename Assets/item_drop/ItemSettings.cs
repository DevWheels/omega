using UnityEngine;


[System.Serializable]
public class ItemSettings
{
    [System.Serializable]
    public class DropChanceSettings
    {
        [Range(0, 100)] public float RankDChance = 40f;
        [Range(0, 100)] public float RankCChance = 30f;
        [Range(0, 100)] public float RankBChance = 20f;
        [Range(0, 100)] public float RankAChance = 8f;
        [Range(0, 100)] public float RankSChance = 2f;
    }

    [System.Serializable]
    public class RankStats
    {
        public Vector2Int baseHealthRange;
        public Vector2Int baseArmorRange;
        public Vector2Int baseAttackRange;
        public int healthPerLevel;
        public int armorPerLevel;
        public int attackPerLevel;
    }

    [System.Serializable]
    public class RankSettings
    {
        public RankStats D;
        public RankStats C;
        public RankStats B;
        public RankStats A;
        public RankStats S;
    }

    public DropChanceSettings dropChances = new DropChanceSettings();
    
    public RankSettings statsByRank;
}