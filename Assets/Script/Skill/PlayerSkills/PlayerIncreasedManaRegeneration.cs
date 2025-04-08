using Unity.VisualScripting;
using UnityEngine;

public class PlayerIncreasedManaRegeneration : Skill {
    
    private RegenerationController regeneration;
    private float baseManaRegeneration;
    public PlayerIncreasedManaRegeneration(RegenerationController regenerationController,SkillConfig skillConfig)
    {
        this.regeneration = regenerationController;
        this.skillConfig = skillConfig;
        this.IsPassive = true;
        baseManaRegeneration = regenerationController.ManaRegeneration;

    }
    public override void Activate()
    {
        
        regeneration.ManaRegenerationPerSecond *= 1 + (skillConfig.PercenageBuff / 100);
    }

    public override void Upgrade()
    {
        
    }

    public override void Deactivate()
    {
        regeneration.ManaRegenerationPerSecond -= baseManaRegeneration;
    }
}
