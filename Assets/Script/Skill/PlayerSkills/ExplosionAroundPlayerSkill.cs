using UnityEngine;

public class ExplosionAroundPlayerSkill : Skill {
    
    private PlayerStats playerStats;
    private GameObject projectilePrefab;
    private Transform SpawnPoint;
    
    public ExplosionAroundPlayerSkill(PlayerStats playerStats,SkillConfig skillConfig)
    {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        projectilePrefab = skillConfig.ProjectilePrefab;
        SpawnPoint = this.playerStats.transform;
        Cooldown = 2f;
    }
    public override void Activate()
    {
        GameObject Explosion = Object.Instantiate(
            projectilePrefab,
            SpawnPoint.position,
            SpawnPoint.rotation

        );
        Explosion.AddComponent<ExplosionAroundPlayerProjectile>();
    }

    public override void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
