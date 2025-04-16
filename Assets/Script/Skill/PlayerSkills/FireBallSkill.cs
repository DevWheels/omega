using UnityEngine;

public class FireBallSkill : Skill {

    private PlayerStats playerStats;
    private GameObject projectilePrefab;
    private Transform SpawnPoint;
    
    public FireBallSkill(PlayerStats playerStats,SkillConfig skillConfig)
    {
        this.playerStats = playerStats;
        this.skillConfig = skillConfig;
        projectilePrefab = skillConfig.ProjectilePrefab;
        SpawnPoint = this.playerStats.transform;
    }


    public override void Activate() {

        GameObject fireball = Object.Instantiate(
            projectilePrefab,
            SpawnPoint.position,
            SpawnPoint.rotation

        );
        fireball.AddComponent<FireBallProjectile>();
        // fireball.GetComponent<FireBallProjectile>().speed = 
        
    }

    public override void Upgrade() {

    }

    public override void Deactivate() {
       
    }
}
