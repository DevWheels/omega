using UnityEngine;

public class PlayerIncreasedSpeed : Skill{

    public float speedIncrease = 1.1f;
    private PlayerMovement playerMovement;
    public PlayerIncreasedSpeed(PlayerMovement PlayerMovement,SkillConfig skillConfig)
    {
        this.playerMovement = PlayerMovement;
        this.skillConfig = skillConfig;
        this.IsPassive = true;
    }

    public override void Activate()
    {
        playerMovement.moveSpeed += speedIncrease;
        this.IsPassive = true;

        Debug.Log(playerMovement.moveSpeed);
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
