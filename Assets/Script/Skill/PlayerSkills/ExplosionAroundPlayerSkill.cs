using Mirror;
using UnityEngine;

public class ExplosionAroundPlayerSkill : Skill {
    
    public ExplosionAroundPlayerSkill(SkillConfig skillConfig, PlayerSkillController playerController) : base(skillConfig,playerController) {
  



    }
    [Command]
    public override void Activate() {
        var explosion = Object.Instantiate(
            skillConfig.ProjectilePrefab,
            playerController.PlayerStats.transform.position,
            playerController.PlayerStats.transform.rotation
        );
        
        NetworkServer.Spawn(explosion.gameObject);
        
        explosion.Init(playerController.gameObject,skillConfig.Damage, skillConfig.ProjectileSpeed,skillConfig.ProjectileLifetime);
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}