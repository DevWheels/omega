using Mirror;
using UnityEngine;

public class FireBallSkill : Skill {
    public FireBallSkill(SkillConfig skillConfig,PlayerSkillController playerController) : base(skillConfig,playerController) { }

    [Command]
    public override void Activate() {
        var fireball = NetworkBehaviour.Instantiate(
            skillConfig.ProjectilePrefab,
            playerController.PlayerStats.transform.position,
            playerController.PlayerStats.transform.rotation
        );
        
        NetworkServer.Spawn(fireball.gameObject);

        fireball.Init(playerController.gameObject, skillConfig.Damage, skillConfig.ProjectileSpeed, skillConfig.ProjectileLifetime);
    }

    public override void Upgrade() { }

    public override void Deactivate() { }
}