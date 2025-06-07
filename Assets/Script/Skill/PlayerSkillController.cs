using System;
using System.Collections;
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

    public PlayerStats PlayerStats { get; private set; }
    public PlayerMovement Playermovement { get; private set; }
    public RegenerationController Regeneration { get; private set; }

    private List<Skill> _skills = new();
    public bool GreenZone = true; // Добавляем булеву переменную (true - город, false - зона скиллов)

    private void Awake() {
        FindComponents();
        GreenZone = true;
        // CreateSkills();
    }

    private void Start() {
        if (isLocalPlayer) {
            InventoryManager.Instance.PlayerSkillController = this;
        }
    }

    private void CreateSkills() {
        foreach (var skillConfig in SkillsTable.Instance.SkillConfigs) {
            var skill = SkillFactory.Create(skillConfig, this);
            _skills.Add(skill);
        }

        SortActiveOrPassiveSkill();
    }

    public void SortActiveOrPassiveSkill() {
        foreach (var skill in _skills) {
            if (skill.skillConfig.IsPassive) {
                Passive_Skills.Add(skill);
            }
            else {
                Active_Skills.Add(skill);
            }

            SkillManager.AddSkill(skill);
        }

        Active_Skills = Active_Skills.DistinctBy(e => e.skillConfig.Name).ToList();
    }

    public void AddNewSkillFromItem() {
        Active_Skills.Clear();
        Passive_Skills.Clear();
        _skills.Clear();

        var newSkills = PlayerEquipment.Instance.GetAllItems();
        foreach (var pair in newSkills) {
            foreach (var t in pair.Value.Skills) {
                var createdSkill = SkillFactory.Create(ConfigsManager.GetSkillConfig(t), this);
                _skills.Add(createdSkill);
            }
        }

        SortActiveOrPassiveSkill();
    }

    public void DeleteSkill(Skill skill) {
        Active_Skills.RemoveAll(s => s.skillConfig.Name == skill.skillConfig.Name);
        Passive_Skills.RemoveAll(s => s.skillConfig.Name == skill.skillConfig.Name);
        _skills.RemoveAll(s => s.skillConfig.Name == skill.skillConfig.Name);

        SkillManager.RemoveSkill(skill);
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

        if (!GreenZone) {
            UseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            GetComponent<PlayerUI>().UpdateUI();
            InventoryManager.Instance.InventoryToggle(GetComponent<PlayerInventory>());
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
        ProjectileBase projectile =
            Instantiate(projectileObject, PlayerStats.transform.position, PlayerStats.transform.rotation);
        projectile.InitDirection(direction);
        projectile.Init(gameObject, config.Damage, config.ProjectileSpeed, config.ProjectileLifetime);
        NetworkServer.Spawn(projectile.gameObject);
    }
    [ClientRpc]
    public void RpcBlockSkills(float duration)
    {
        StartCoroutine(BlockSkillsRoutine(duration));
    }

    private IEnumerator BlockSkillsRoutine(float duration)
    {
        bool canUseSkills = false; // Предполагаем, что у вас есть такое поле
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            yield return null;
        }
        canUseSkills = true;
    }
    public void TeleportToArena() {
        GreenZone = false;
    }

    public void TeleportToGreenZone() {
        GreenZone = true;
    }
}