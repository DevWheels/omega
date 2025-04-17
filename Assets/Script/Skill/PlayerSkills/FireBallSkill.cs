using UnityEngine;

public class FireBallSkill : Skill {
    public FireBallSkill(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig,playerController) { }
    
    public override void Activate(Vector3 mousePosition) {
        Vector3 targetDirection = (mousePosition - playerController.transform.position).normalized;
        playerController.SpawnProjectile(skillConfig,targetDirection);
     }

    public override void Upgrade() { }

    public override void Deactivate() { }
}