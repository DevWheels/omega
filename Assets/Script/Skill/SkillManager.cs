using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour {
    public List<Skill> Skills = new List<Skill>();
    private List<Skill> SkillCooldowns = new List<Skill>();

    public void AddSkill(Skill skill) {
        Skills.Add(skill);
        if (skill.skillConfig.IsPassive)
        {
            skill.Activate();
        }
    }

    public void UseSkill(Skill playerskill) {
        Skill skill = Skills.Find(s => s == playerskill);

        if (skill == null || SkillCooldowns.Contains(skill)) {
            return;
        }
        
        skill.Activate();
        StartCoroutine(CooldownSkill(skill));
    }

    private IEnumerator CooldownSkill(Skill skill) {
        SkillCooldowns.Add(skill);
        yield return new WaitForSeconds(skill.skillConfig.Couldown);
        SkillCooldowns.Remove(skill);
    }
}