using Mirror;

public class HealingSkill : Skill{
    public HealingSkill(SkillConfig skillConfig, PlayerSkillController playerController) : base(skillConfig, playerController) { }

    public override void Activate() {
        var healing = NetworkBehaviour.Instantiate(
            skillConfig.ProjectilePrefab,
            playerController.PlayerStats.transform.position,
            playerController.PlayerStats.transform.rotation
        );
        
        NetworkServer.Spawn(healing.gameObject);

        healing.Init(playerController.gameObject, skillConfig.Damage, skillConfig.ProjectileSpeed, skillConfig.ProjectileLifetime);
        playerController.PlayerStats.CurrentlyHp += skillConfig.Damage;
    }
    public override void Deactivate() {
        
    }
}
