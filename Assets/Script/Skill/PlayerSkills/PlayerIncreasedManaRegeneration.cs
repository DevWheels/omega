using Unity.VisualScripting;
using UnityEngine;

public class PlayerIncreasedManaRegeneration : Skill {

    public float ManaIncrease = 1.1f;
    private PlayerStats playerStats;
    
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats,SkillConfig skillConfig)
    {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;

    }
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }
    public override void Activate()
    {
        playerStats.ManaRegenerationPerSecond += ManaIncrease;
    }

    public override void Upgrade()
    {
    }

    public override void Deactivate()
    {
        playerStats.ManaRegenerationPerSecond -= ManaIncrease;
    }
}
