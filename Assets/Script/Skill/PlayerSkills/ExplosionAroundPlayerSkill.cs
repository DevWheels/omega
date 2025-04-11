using Unity.VisualScripting;
using UnityEngine;

public class ExplosionAroundPlayerSkill : Skill {
    private PlayerStats playerStats;
    private ProjectileBase projectilePrefab;
    private Transform SpawnPoint;

    public ExplosionAroundPlayerSkill(PlayerStats playerStats, SkillConfig skillConfig) {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        projectilePrefab = skillConfig.ProjectilePrefab;
        SpawnPoint = this.playerStats.transform;

    }

    public override void Activate() {
        var explosion = Object.Instantiate(
            projectilePrefab,
            SpawnPoint.position,
            SpawnPoint.rotation
        );
        explosion.Init(skillConfig.Damage, skillConfig.ProjectileSpeed,skillConfig.ProjectileLifetime);
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}