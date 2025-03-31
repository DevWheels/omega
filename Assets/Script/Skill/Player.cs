using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
        public int ap = 0;
        public List<Equipment> equippedItems = new List<Equipment>();
        public List<Skill> availableSkills = new List<Skill>();
        public List<Skill> masteredSkills = new List<Skill>();

      
        public void AddAP(int amount)
        {
            ap += amount;
       
        }

      
        public void Equip(Equipment equipment)
        {
            if (!equippedItems.Contains(equipment))
            {
                equippedItems.Add(equipment);
                equipment.Equip(this);
                UpdateSkillsUI();
            }
        }

        public void Unequip(Equipment equipment)
        {
            if (equippedItems.Contains(equipment))
            {
                equippedItems.Remove(equipment);
                equipment.Unequip(this);
                UpdateSkillsUI();
            }
        }

     
        public void AddAvailableSkill(Skill skill)
        {
            if (!availableSkills.Contains(skill) && !masteredSkills.Contains(skill))
            {
                availableSkills.Add(skill);
            }
        }

        public void RemoveAvailableSkill(Skill skill)
        {
            if (skill.state != SkillState.Mastered && availableSkills.Contains(skill))
            {
                availableSkills.Remove(skill);
            }
            else if (skill.state == SkillState.Mastered && availableSkills.Contains(skill))
            {
                availableSkills.Remove(skill);
                masteredSkills.Add(skill);
            }
        }


        public bool UpgradeSkill(Skill skill)
        {
            if (availableSkills.Contains(skill) && ap >= skill.apCost[skill.level])
            {
                if (skill.Upgrade())
                {
                    ap -= skill.apCost[skill.level];
                    if (skill.state == SkillState.Mastered)
                    {
                        availableSkills.Remove(skill);
                        masteredSkills.Add(skill);
                    }
                    UpdateSkillsUI();
                    return true;
                }
            }
            return false;
        }


        private void UpdateSkillsUI()
        {
            SkillsMenu skillsMenu = FindObjectOfType<SkillsMenu>();
            if (skillsMenu != null)
            {
                skillsMenu.UpdateSkillsDisplay();
            }
        }
    
}
