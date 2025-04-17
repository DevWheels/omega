using Mirror;
using UnityEngine;

public class ExplosionAroundPlayerSkill : Skill {
    public ExplosionAroundPlayerSkill(SkillConfig skillConfig, PlayerSkillController playerController) : base(skillConfig, playerController) { }

    public override void Activate(Vector3 mousePosition) {
        playerController.SpawnProjectile(skillConfig, Vector3.zero);
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}