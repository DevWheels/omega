using Unity.VisualScripting;
using UnityEngine;

public class FireBallSkill : Skill {
    public FireBallSkill(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig,playerController) { }


    public override void Activate() {
        var fireball = Object.Instantiate(
            skillConfig.ProjectilePrefab,
            playerController.PlayerStats.transform.position,
            playerController.PlayerStats.transform.rotation
        );

        fireball.Init(skillConfig.Damage, skillConfig.ProjectileSpeed, skillConfig.ProjectileLifetime);
    }

    public override void Upgrade() { }

    public override void Deactivate() { }
}