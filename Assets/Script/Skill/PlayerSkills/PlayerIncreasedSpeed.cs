using UnityEngine;

public class PlayerIncreasedSpeed : Skill {

    private float baseSpeed;

    public PlayerIncreasedSpeed(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig,playerController) {

        baseSpeed = playerController.Playermovement.moveSpeed;
    }

    public override void Activate(Vector3 mousePosition)
    {
        playerController.Playermovement.moveSpeed *= 1 + (skillConfig.PercentageBuff / 100f);
    }

    public override void Upgrade() {
    }

    public override void Deactivate()
    {
        playerController.Playermovement.moveSpeed -= baseSpeed;
    }
}
