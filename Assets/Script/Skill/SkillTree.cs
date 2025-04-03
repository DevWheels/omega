using System.Collections.Generic;
using UnityEngine;
public class SkillTree : MonoBehaviour
{
    public List<Skill> AvailableSkills { get; private set; }  //Все навыки
    public List<Skill> UnlockedSkills { get; private set; }   //Открытые навыки
    
    public SkillTree() {
        AvailableSkills = new List<Skill>();
        UnlockedSkills = new List<Skill>();
    }
    
    public void UnlockSkill(Skill skill) {
        if (!AvailableSkills.Contains(skill) || UnlockedSkills.Contains(skill)) return;
        
        UnlockedSkills.Add(skill);
        skill.Activate();
    }
}