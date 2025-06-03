using System;
using System.Collections.Generic;

using UnityEngine;

public class ConfigsManager : MonoBehaviour{
        private static ConfigsManager instance;
        
        [SerializeField] private List<SkillConfig> _skills;
        

        private void Awake() {
                instance = this;
        }

        public static SkillConfig GetSkillConfig(SkillType skillType) {
                return instance._skills.Find(skill => skill.SkillType == skillType);
        }
}
