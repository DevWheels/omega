using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemSkill", menuName = "Skill/ItemSkills")]
public class ItemSkillsConfig : ScriptableObject {
        public List<SkillConfig> ItemSkills = new();
}