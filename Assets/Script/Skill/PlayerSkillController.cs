using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour 
{
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    private PlayerStats playerStats;
    private PlayerMovement playermovement;


    private void Awake() 
    {

        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = gameObject.AddComponent<SkillTree>();
        playerStats = GetComponent<PlayerStats>();
        playermovement = GetComponent<PlayerMovement>();


        var manaRegenSkill = new PlayerIncreasedManaRegeneration(playerStats,Resources.Load<SkillConfig>($"ManaIncrease"));
        var SpeedIncreaseSkill = new PlayerIncreasedSpeed(playermovement,Resources.Load<SkillConfig>($"SpeedIncrease"));
        var fireBallSkill = new FireBallSkill(playerStats,Resources.Load<SkillConfig>($"Skills/FireBall"));



        Active_Skills.Add(fireBallSkill);
        SkillManager.Skills.Add(fireBallSkill);
        
        Passive_Skills.Add(manaRegenSkill);
        SkillManager.AddSkill(manaRegenSkill);
        
        Passive_Skills.Add(SpeedIncreaseSkill);
        SkillManager.AddSkill(SpeedIncreaseSkill);
        
    }

    private void Update() {
        UseSkill();
    }

    private void UseSkill() {
        if (Input.GetKey(KeyCode.Q)) {
            SkillManager.UseSkill(Active_Skills[0]);
            Active_Skills[0].Activate();
        }
    }
}