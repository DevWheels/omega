using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Skill
{
    public class SkillManager : MonoBehaviour
    {
        public List<Skill> Skills = new List<Skill>();
        private Dictionary<string, Skill> SkillCooldowns = new Dictionary<string, Skill>();

        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);
        }

        public void UseSkill(string skillName)
        {
            Skill skill = Skills.Find(s => s.Name == skillName);
            if (skill != null && !SkillCooldowns.ContainsKey(skillName))
            {
                skill.Activate();
                StartCoroutine(CooldownSkill(skill));
            }
        }

        private IEnumerator CooldownSkill(Skill skill)
        {
            SkillCooldowns.Add(skill.Name, skill);
            yield return new WaitForSeconds(skill.Cooldown);
            SkillCooldowns.Remove(skill.Name);
        }
    }
}