using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    private PlayerStats playerStats;
    private PlayerMovement playermovement;
    private RegenerationController regeneration;

    [SerializeField]
    private List<SkillConfig> _skillConfigs;

    private void Awake() {
        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = gameObject.AddComponent<SkillTree>();
        playerStats = GetComponent<PlayerStats>();
        playermovement = GetComponent<PlayerMovement>();
        regeneration = GetComponent<RegenerationController>();

        var manaRegenSkill = new PlayerIncreasedManaRegeneration(regeneration, _skillConfigs.First(s => s.Name == "ManaIncrease"));
        var speedIncreaseSkill = new PlayerIncreasedSpeed(playermovement, _skillConfigs.First(s => s.Name == "SpeedIncrease"));
        var fireBallSkill = new FireBallSkill(playerStats, _skillConfigs.First(s => s.Name == "FireBall"));
        var explosionSkill = new ExplosionAroundPlayerSkill(playerStats, _skillConfigs.First(s => s.Name == "ExplosionAroundPlayer"));
        List<Skill> usedSkills = new List<Skill> {
            manaRegenSkill,
            speedIncreaseSkill,
            fireBallSkill,
            explosionSkill
        };

        foreach (var skill in usedSkills) {
            if (skill.skillConfig.IsPassive) {
                Passive_Skills.Add(skill);
            }
            else { Active_Skills.Add(skill); }
            SkillManager.AddSkill(skill);
        }


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