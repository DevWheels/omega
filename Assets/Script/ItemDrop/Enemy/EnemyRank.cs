using UnityEngine;

public enum EnemyRank
{
    Common,    
    Elite,     
    Champion,  
    Boss,      
    Legendary  
}
public enum ItemRank { D, C, B, A, S }
public enum ItemType {
    helmet, shoulderPads, bracers, chestPlate, boots, weapon, bracelet, necklace }
public enum SpecialStatType
{
    Dodge, CRIT, DodgeRES, CRITRES,
    HPSteel, BoostCRITDMG, ReduceCRITDMG,
    RestoreHP, BoostDMG
}
[System.Serializable]
public struct RankStatRanges
{
    public Vector2Int health;
    public Vector2Int armor;
    public Vector2Int attack;
    public int perLevelIncrease;
}