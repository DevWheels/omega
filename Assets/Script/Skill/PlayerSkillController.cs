using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerSkillController : NetworkBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    public PlayerStats PlayerStats {get; private set;}
    public PlayerMovement Playermovement{get; private set;}
    public RegenerationController Regeneration{get; private set;}

    private List<Skill> _skills = new();
    private void Awake() {
        FindComponents();

        CreateSkills();
    }

    private void CreateSkills() {
        foreach (var skillConfig in SkillsTable.Instance.SkillConfigs) {
            var skill = SkillFactory.Create(skillConfig,this);
            _skills.Add(skill);
        }

        foreach (var skill in _skills) {
            if (skill.skillConfig.IsPassive) {
                Passive_Skills.Add(skill);
            }
            else { Active_Skills.Add(skill); }
            SkillManager.AddSkill(skill);
        }
    }

    
    private void FindComponents() {
        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = new SkillTree();
        PlayerStats = GetComponent<PlayerStats>();
        Playermovement = GetComponent<PlayerMovement>();
        Regeneration = GetComponent<RegenerationController>();
    }


    private void Update() {
        if (!isLocalPlayer) {
            return;
        }
        UseSkill();
    }

    private void UseSkill() {
        if (Input.GetKey(KeyCode.Q)) {
            SkillManager.UseSkill(Active_Skills[0]);
        }
        else if (Input.GetKey(KeyCode.E)) {
            SkillManager.UseSkill(Active_Skills[1]);
        }
    }
    
    [Command]
    public void SpawnProjectile(SkillConfig config) {
        var projectileObject = ProjectileFactory.Instance.GetProjectileByType(config.ProjectileType);
        ProjectileBase projectile = Instantiate(projectileObject, PlayerStats.transform.position, PlayerStats.transform.rotation);
        projectile.Init(gameObject, config.Damage, config.ProjectileSpeed, config.ProjectileLifetime);
        NetworkServer.Spawn(projectile.gameObject);
    }
}