using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillController : NetworkBehaviour {
   

    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; set; } = new List<Skill>();

    public PlayerStats PlayerStats {get; private set;}
    public PlayerMovement Playermovement{get; private set;}
    public RegenerationController Regeneration{get; private set;}

    private List<Skill> _skills = new();
    private bool _greenZone = true; // Добавляем булеву переменную (true - город, false - зона скиллов)
    private void Awake() {
        FindComponents();
        _greenZone = true;
        // CreateSkills();
    }

    private void CreateSkills() {
        foreach (var skillConfig in SkillsTable.Instance.SkillConfigs) {
            var skill = SkillFactory.Create(skillConfig,this);
            _skills.Add(skill);
        }

        SortActiveOrPassiveSkill();

    }

    private void SortActiveOrPassiveSkill() {
        foreach (var skill in _skills) {
            if (skill.skillConfig.IsPassive) {
                Passive_Skills.Add(skill);
            }
            else { Active_Skills.Add(skill); }
            SkillManager.AddSkill(skill);
            Active_Skills = Active_Skills.DistinctBy(e => e.skillConfig.Name).ToList();
        }
    }
    public void AddNewSkillFromItem(Skill skill) {
        _skills.Add(skill);
        SkillManager.AddSkill(skill);
        
        SortActiveOrPassiveSkill();
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
        if (!_greenZone)
        {
            UseSkill();
        }
    }

    private void UseSkill() {
        if (Input.GetKey(KeyCode.Q) && Active_Skills.Count > 0) {
            SkillManager.UseSkill(Active_Skills[0]);
        }

        if (Input.GetKey(KeyCode.E) && Active_Skills.Count > 1) {
            SkillManager.UseSkill(Active_Skills[1]);
        }
    }
    
    [Command]
    public void SpawnProjectile(SkillConfig config, Vector3 direction) {
        ProjectileBase projectileObject = ProjectileFactory.Instance.GetProjectileByType(config.ProjectileType);
        ProjectileBase projectile = Instantiate(projectileObject, PlayerStats.transform.position, PlayerStats.transform.rotation);
        projectile.InitDirection(direction);
        projectile.Init(gameObject, config.Damage, config.ProjectileSpeed, config.ProjectileLifetime);
        NetworkServer.Spawn(projectile.gameObject);
    }

    public void TeleportToArena() {
        _greenZone = false;
    }
}