using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillSelectorHandler : MonoBehaviour {
    private void Start() {


        foreach (var button in GameUI.Instance.SkillContainerView.buttons) {
            button.Disable();
        }
    }

    public void UpdateSkillSelector() {
        var allSkills = PlayerEquipment.Instance.GetAllItems();
        List<SkillConfig> skillConfigs = new();
        
        foreach (var item in allSkills) {
            
            foreach (var skillName in item.Value.Skills) {
                skillConfigs.Add(ConfigsManager.GetSkillConfig(skillName));
            }
        }

        for (int i = 0; i < skillConfigs.Count && i < GameUI.Instance.SkillContainerView.buttons.Count; i++) {

            GameUI.Instance.SkillContainerView.buttons[i].Enable();
            GameUI.Instance.SkillContainerView.buttons[i].SetData(skillConfigs[i],SelectSkill,DeselectSkill);

        }

    }

    public void SelectSkill(SkillConfig skillConfig) {

        InventoryManager.Instance.PlayerSkillController.AddNewSkillFromItem();
    }

    public void DeselectSkill(SkillConfig skillConfig) {
        InventoryManager.Instance.PlayerSkillController.DeleteSkill(SkillFactory.Create(skillConfig,InventoryManager.Instance.PlayerSkillController));
    }

}