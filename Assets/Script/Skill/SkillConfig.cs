using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/SkillConfig")]
public class SkillConfig : ScriptableObject {
    public Sprite skillIcon;
    public string Name;
    [TextArea]
    public string Description;
    
    [Header("Skill Type")]
    public bool IsPassive;
    public SkillType SkillType;
    
    [Header("Passive Settings")]
    public int PercentageBuff;
    
    [Header("Active Settings")]
    public int Damage;
    public int ProjectileSpeed;
    [Tooltip("Cooldown in seconds")]
    public int Cooldown;
    public int ProjectileLifetime;

    public ProjectileType ProjectileType;
}
[System.Serializable]
public enum SkillType {
    FireBallSkill,
    ExplosionAroundPlayerSkill,
    ManaIncreaseSkill,
    SpeedIncreaseSkill,
    FireBallSkillSmall,
    HealingSkill,
    
}