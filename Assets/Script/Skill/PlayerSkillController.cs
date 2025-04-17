using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour 
{
    public bool greenZone = true; // Добавляем булеву переменную (true - город, false - зона скиллов)
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }
    public List<Skill> Passive_Skills { get; } = new List<Skill>();
    public List<Skill> Active_Skills { get; } = new List<Skill>();

    public PlayerStats PlayerStats { get; private set; }
    public PlayerMovement Playermovement { get; private set; }
    public RegenerationController Regeneration { get; private set; }

    private List<Skill> _skills = new();

    private void Awake() 
    {
        FindComponents();
        CreateSkills();
        
        // Устанавливаем начальное значение (город - нельзя использовать скиллы)
        greenZone = true;
    }

    // Остальной код без изменений...
    private void CreateSkills() 
    {
        foreach (var skillConfig in SkillsTable.Instance.SkillConfigs) 
        {
            var skill = SkillFactory.Create(skillConfig, this);
            _skills.Add(skill);
        }

        foreach (var skill in _skills) 
        {
            if (skill.skillConfig.IsPassive) 
            {
                Passive_Skills.Add(skill);
            }
            else 
            { 
                Active_Skills.Add(skill); 
            }
            SkillManager.AddSkill(skill);
        }
    }

    private void FindComponents() 
    {
        SkillManager = gameObject.AddComponent<SkillManager>();
        SkillTree = new SkillTree();
        PlayerStats = GetComponent<PlayerStats>();
        Playermovement = GetComponent<PlayerMovement>();
        Regeneration = GetComponent<RegenerationController>();
    }

    private void Update() 
    {
        if (!Playermovement.isLocalPlayer) 
        {
            return;
        }
        if (!greenZone)
        {
            UseSkill();
        }
    }

    private void UseSkill() 
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            SkillManager.UseSkill(Active_Skills[0]);
        }
        else if (Input.GetKey(KeyCode.E)) 
        {
            SkillManager.UseSkill(Active_Skills[1]);
        }
    }
}