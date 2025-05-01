using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectorButton : MonoBehaviour {
        private SkillConfig _skillConfig;
        private Action<SkillConfig> _onSelected;
        public void SetData(SkillConfig skillConfig,Action<SkillConfig> onSelected ) {
                _skillConfig = skillConfig;
                _onSelected = onSelected;
                GetComponent<Image>().sprite = _skillConfig.skillIcon;
        }
        public void AddSkill() {
                _onSelected.Invoke(_skillConfig);
        }

        public void Disable() {
                gameObject.SetActive(false);
        }

        public void Enable() {
                gameObject.SetActive(true);
        }
}
