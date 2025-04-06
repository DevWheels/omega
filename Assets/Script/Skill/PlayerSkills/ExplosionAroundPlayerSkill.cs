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
        Cooldown = 2f;
    }

    public override void Activate() {
        ProjectileBase explosion = Object.Instantiate(
            projectilePrefab,
            SpawnPoint.position,
            SpawnPoint.rotation
        );
        explosion.AddComponent<ExplosionAroundPlayerProjectile>();
    }

    public override void Upgrade() {
        throw new System.NotImplementedException();
    }

    public override void Deactivate() {
        throw new System.NotImplementedException();
    }
}