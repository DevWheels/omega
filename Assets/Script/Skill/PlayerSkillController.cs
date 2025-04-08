using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    private PlayerStats playerStats;
    private PlayerMovement playermovement;
    private RegenerationController regeneration;

    private void Awake() {
        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = gameObject.AddComponent<SkillTree>();
        playerStats = GetComponent<PlayerStats>();
        playermovement = GetComponent<PlayerMovement>();
        regeneration = GetComponent<RegenerationController>();

        var manaRegenSkill =
            new PlayerIncreasedManaRegeneration(regeneration, Resources.Load<SkillConfig>($"Skills/ManaIncrease"));
        var SpeedIncreaseSkill =
            new PlayerIncreasedSpeed(playermovement, Resources.Load<SkillConfig>($"Skills/SpeedIncrease"));
        var fireBallSkill = new FireBallSkill(playerStats, Resources.Load<SkillConfig>($"Skills/FireBall"));
        var ExplosionSkill =
            new ExplosionAroundPlayerSkill(playerStats, Resources.Load<SkillConfig>($"Skills/ExplosionAroundPlayer"));
        List<Skill> usedSkills = new List<Skill> {
            manaRegenSkill,
            SpeedIncreaseSkill,
            fireBallSkill,
            ExplosionSkill
        };

        foreach (var skill in usedSkills) {
            if (skill.skillConfig.isPassive) {
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