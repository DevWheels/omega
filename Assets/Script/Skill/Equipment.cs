using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Inventory/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipmentName;
    public Sprite icon;
    public List<Skill> attachedSkills = new List<Skill>();

    public void Equip(Player1 player)
    {
        foreach (var skill in attachedSkills)
        {
            if (skill.state != SkillState.Mastered)
            {
                skill.state = SkillState.Unlocked;
            }
            player.AddAvailableSkill(skill);
        }
    }

    public void Unequip(Player1 player)
    {
        foreach (var skill in attachedSkills)
        {
            if (skill.state != SkillState.Mastered)
            {
                skill.state = SkillState.Locked;
                skill.level = 0;
            }
            player.RemoveAvailableSkill(skill);
        }
    }
}