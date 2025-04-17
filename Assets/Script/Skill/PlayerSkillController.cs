using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; set; } = new List<Skill>();

    public PlayerStats PlayerStats {get; private set;}
    public PlayerMovement Playermovement{get; private set;}
    public RegenerationController Regeneration{get; private set;}

    private List<Skill> _skills = new();
    private void Awake() {
        FindComponents();

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
        if (!Playermovement.isLocalPlayer) {
            return;
        }
        UseSkill();
    }

    private void UseSkill() {
        if (Active_Skills == null || Active_Skills.Count == 0) { return; }


        if (Input.GetKey(KeyCode.Q)) {
            SkillManager.UseSkill(Active_Skills[0]);
        }
        else if (Input.GetKey(KeyCode.E)) {
            SkillManager.UseSkill(Active_Skills[1]);
        }
    }
}