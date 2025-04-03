using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour 
{
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Skills { get; } = new List<Skill>();

    private PlayerStats playerStats;


    private void Awake() 
    {

        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = gameObject.AddComponent<SkillTree>();
        playerStats = GetComponent<PlayerStats>();


        var manaRegenSkill = new PlayerIncreasedManaRegeneration(playerStats,Resources.Load<SkillConfig>($"ManaIncrease"));
        Skills.Add(manaRegenSkill);
        SkillManager.AddSkill(manaRegenSkill);
        
    }
}