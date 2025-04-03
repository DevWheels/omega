using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public List<Skill> Skills = new List<Skill>();
    private List<Skill> SkillCooldowns = new List<Skill>();

    public void AddSkill(Skill skill) {
        Skills.Add(skill);
    }

    public void UseSkill(string skillName) {
        Skill skill = Skills.Find(s => s.skillConfig.Name == skillName);
        if (skill == null || SkillCooldowns.Contains(skill)) {return;}
        skill.Activate();
        StartCoroutine(CooldownSkill(skill));
    }

    private IEnumerator CooldownSkill(Skill skill) {
        SkillCooldowns.Add(skill);
        yield return new WaitForSeconds(skill.Cooldown);
        SkillCooldowns.Remove(skill);
    }
}
