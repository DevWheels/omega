using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectorButton : MonoBehaviour {
        private SkillConfig _skillConfig;
        private Action<SkillConfig> _onSelected;
        private Action<SkillConfig> _onDeselected;

        private bool _isSelected = false;
        public void SetData(SkillConfig skillConfig,Action<SkillConfig> onSelected,Action<SkillConfig> onDeselected ) {
                _skillConfig = skillConfig;
                _onSelected = onSelected;
                _onDeselected = onDeselected;
                GetComponent<Image>().sprite = _skillConfig.skillIcon;
        }
        public void ToggleSkill() 
        {
            if (!_isSelected)
            {
                _onSelected?.Invoke(_skillConfig);
                _isSelected = true;
            }
            else
            {
                _onDeselected?.Invoke(_skillConfig);
                _isSelected = false;
            }
        }

        public void Disable() {
                gameObject.SetActive(false);
        }

        public void Enable() {
                gameObject.SetActive(true);
        }
}
