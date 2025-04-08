using Unity.VisualScripting;
using UnityEngine;

public class FireBallSkill : Skill {
    private PlayerStats playerStats;
    private ProjectileBase projectilePrefab;
    private Transform SpawnPoint;

    public FireBallSkill(PlayerStats playerStats, SkillConfig skillConfig) {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        projectilePrefab = skillConfig.ProjectilePrefab;
        SpawnPoint = this.playerStats.transform;
    }


    public override void Activate() {
        var fireball = Object.Instantiate(
            projectilePrefab,
            SpawnPoint.position,
            SpawnPoint.rotation
        );

        fireball.Init(skillConfig.Damage, skillConfig.Speed, skillConfig.ProjectileLifetime);
    }

    public override void Upgrade() { }

    public override void Deactivate() { }
}