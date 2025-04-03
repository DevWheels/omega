using Unity.VisualScripting;
using UnityEngine;

public class PlayerIncreasedManaRegeneration : Skill {

    public float ManaIncrease = 1.1f;
    private PlayerStats playerStats;
    
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats,SkillConfig skillConfig)
    {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        this.IsPassive = true;

    }
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
        this.IsPassive = true;
    }
    public override void Activate()
    {
        playerStats.ManaRegenerationPerSecond += ManaIncrease;
    }

    public override void Upgrade()
    {
        this.Level += 1;
    }
}
