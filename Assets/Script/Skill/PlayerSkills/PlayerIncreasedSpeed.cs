using UnityEngine;

public class PlayerIncreasedSpeed : Skill {

    private float baseSpeed;
    private PlayerMovement playerMovement;
    public PlayerIncreasedSpeed(PlayerMovement PlayerMovement,SkillConfig skillConfig)
    {
        this.playerMovement = PlayerMovement;
        this.skillConfig = skillConfig;
        skillConfig.IsPassive = true;
        baseSpeed = PlayerMovement.moveSpeed;
    }

    public override void Activate()
    {
        playerMovement.moveSpeed *= 1 + (skillConfig.PercentageBuff / 100);
    }

    public override void Upgrade() {
    }

    public override void Deactivate()
    {
        playerMovement.moveSpeed -= baseSpeed;
    }
}
