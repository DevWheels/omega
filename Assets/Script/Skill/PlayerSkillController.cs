using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSkillController : MonoBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    public PlayerStats PlayerStats {get; private set;}
    public PlayerMovement Playermovement{get; private set;}
    public RegenerationController Regeneration{get; private set;}

    [SerializeField]
    private List<SkillConfig> _skillConfigs;

    private List<Skill> _skills = new();
    private void Awake() {
        FindComponents();

        CreateSkills();
    }

    private void CreateSkills() {
        foreach (var skillConfig in _skillConfigs) {
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
        SkillTree = gameObject.AddComponent<SkillTree>();
        PlayerStats = GetComponent<PlayerStats>();
        Playermovement = GetComponent<PlayerMovement>();
        Regeneration = GetComponent<RegenerationController>();
    }


    private void Update() {
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
}