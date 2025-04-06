using UnityEngine;

public class PlayerIncreasedSpeed : Skill {

    private float baseSpeed;
    private PlayerMovement playerMovement;
    public PlayerIncreasedSpeed(PlayerMovement PlayerMovement,SkillConfig skillConfig)
    {
        this.playerMovement = PlayerMovement;
        this.skillConfig = skillConfig;
        this.IsPassive = true;
        baseSpeed = PlayerMovement.moveSpeed;
    }

    public override void Activate()
    {
        playerMovement.moveSpeed *= 1 + (skillConfig.PercenageBuff / 100);
        IsPassive = true;
    }

    public override void Upgrade()
    {
        Level += 1;
    }

    public override void Deactivate()
    {
        playerMovement.moveSpeed -= baseSpeed;
    }
}
