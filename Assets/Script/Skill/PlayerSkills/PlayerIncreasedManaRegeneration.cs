using Unity.VisualScripting;
using UnityEngine;

public class PlayerIncreasedManaRegeneration : Skill {
    
    private PlayerStats playerStats;
    private float baseManaRegeneration;
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats,SkillConfig skillConfig)
    {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        this.IsPassive = true;
        baseManaRegeneration = playerStats.ManaRegeneration;

    }
    public PlayerIncreasedManaRegeneration(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
        this.IsPassive = true;
    }
    public override void Activate()
    {
        
        playerStats.ManaRegenerationPerSecond *= 1 + (skillConfig.PercenageBuff / 100);
    }

    public override void Upgrade()
    {
        
    }

    public override void Deactivate()
    {
        playerStats.ManaRegenerationPerSecond -= baseManaRegeneration;
    }
}
