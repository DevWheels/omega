using Mirror;
using UnityEngine;

public class HealingSkill : Skill{
    public HealingSkill(SkillConfig skillConfig, PlayerSkillController playerController) : base(skillConfig, playerController) { }
    

    public override void Activate(Vector3 mousePos) {
        playerController.SpawnProjectile(skillConfig,Vector3.zero);
        playerController.PlayerStats.CurrentlyHp += skillConfig.Damage;
    }

    public override void Deactivate() {
        
    }
}
