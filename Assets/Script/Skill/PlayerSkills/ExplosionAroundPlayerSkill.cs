using Mirror;
using UnityEngine;

public class ExplosionAroundPlayerSkill : Skill {
    public ExplosionAroundPlayerSkill(SkillConfig skillConfig, PlayerSkillController playerController) : base(skillConfig, playerController) { }

    public override void Activate() {
        playerController.SpawnProjectile(skillConfig);
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}