using UnityEngine;

public class PlayerIncreasedSpeed : Skill{

    public float speedIncrease = 0.1f;
    private PlayerMovement playerMovement;
    public PlayerIncreasedSpeed(PlayerMovement PlayerMovement,SkillConfig skillConfig)
    {
        this.playerMovement = PlayerMovement;
        this.skillConfig = skillConfig;
    }

    public override void Activate()
    {
        playerMovement.moveSpeed += speedIncrease;
    }

    public override void Upgrade()
    {
        this.Level += 1;
    }

    public override void Deactivate()
    {
        playerMovement.moveSpeed -= speedIncrease;
    }
}
