using Unity.VisualScripting;
using UnityEngine;

public class PlayerIncreasedManaRegeneration : Skill {
    
    private float baseManaRegeneration;
    public PlayerIncreasedManaRegeneration(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig, playerController)
    {


        baseManaRegeneration = playerController.Regeneration.ManaRegeneration;

    }
    public override void Activate()
    {
        
        playerController.Regeneration.ManaRegenerationPerSecond *= 1 + (skillConfig.PercentageBuff / 100f);
    }

    public override void Upgrade()
    {
        
    }

    public override void Deactivate()
    {
        playerController.Regeneration.ManaRegenerationPerSecond -= baseManaRegeneration;
    }
}
