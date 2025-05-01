using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillSelectorHandler : MonoBehaviour {
    [SerializeField] private GameObject skillContainer;
    // [SerializeField] private GameObject _gameObjectEquipment;

    

    private void Start() {
        // equipment = _gameObjectEquipment.GetComponent<PlayerEquipment>();

        foreach (var button in GameUI.Instance.SkillContainerView.buttons) {
            button.Disable();
        }
    }

    public void UpdateSkillSelector() {
        var allSkills = PlayerEquipment.Instance.GetAllItems();
        List<SkillConfig> skillConfigs = new();
        
        foreach (var item in allSkills) {
            
            foreach (var skillConfig in item.Value.itemSkills) {
                skillConfigs.Add(skillConfig);
            }
        }

        for (int i = 0; i < skillConfigs.Count && i < GameUI.Instance.SkillContainerView.buttons.Count; i++) {
            Debug.Log(GameUI.Instance.SkillContainerView.buttons[i].name);

            GameUI.Instance.SkillContainerView.buttons[i].Enable();
            GameUI.Instance.SkillContainerView.buttons[i].SetData(skillConfigs[i],SelectSkill);

        }

    }

    public void SelectSkill(SkillConfig skillConfig) {

        InventoryManager.Instance.PlayerSkillController.AddNewSkillFromItem();
    }

}