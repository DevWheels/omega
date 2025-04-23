using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour {
    public List<Skill> Skills = new List<Skill>();
    private List<Skill> SkillCooldowns = new List<Skill>();

    public void AddSkill(Skill skill) {
        foreach (var itemSkill in Skills) {
            if (skill == itemSkill) {
                return;
            }
        }
        Skills.Add(skill);
        if (skill.skillConfig.IsPassive)
        {
            skill.Activate(GetMouseWorldPosition());
        }
    }

    public void UseSkill(Skill playerskill) {
        Skill skill = Skills.Find(s => s == playerskill);

        if (skill == null || SkillCooldowns.Contains(skill)) {
            return;
        }
        
        skill.Activate(GetMouseWorldPosition());
        StartCoroutine(CooldownSkill(skill));
    }

    private IEnumerator CooldownSkill(Skill skill) {
        SkillCooldowns.Add(skill);
        yield return new WaitForSeconds(skill.skillConfig.Cooldown);
        SkillCooldowns.Remove(skill);
    }
    
    private Vector3 GetMouseWorldPosition() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}