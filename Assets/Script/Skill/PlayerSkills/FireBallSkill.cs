using UnityEngine;

public class FireBallSkill : Skill {
    public FireBallSkill(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig,playerController) { }
    
    public override void Activate() {
        playerController.SpawnProjectile(skillConfig);
     }

    public override void Upgrade() { }

    public override void Deactivate() { }
}