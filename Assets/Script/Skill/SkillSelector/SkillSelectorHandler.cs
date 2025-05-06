using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillSelectorHandler : MonoBehaviour {
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private List<GameObject> buttons = new();

    public void UpdateSkillSelector() {
        var allSkills = equipment.GetAllItems();
        List<SkillConfig> skillConfigs = new();
        
        foreach (var item in allSkills) {
            foreach (var skillConfig in item.Value.itemSkills) {
                skillConfigs.Add(skillConfig);
            }
        }


        foreach (var button in buttons) {
            button.SetActive(false);
        }


        for (int i = 0; i < skillConfigs.Count && i < buttons.Count; i++) {
            buttons[i].GetComponent<Image>().sprite = skillConfigs[i].skillIcon;
            buttons[i].SetActive(true);
            
            buttons[i].GetComponent<Button>().onClick.AddListener(AddSkillIntoPlayerSkills(skillConfigs[i]));
        }

    }

    private UnityAction AddSkillIntoPlayerSkills(SkillConfig skillConfig) {
        return () => InventoryManager.Instance.PlayerSkillController.SkillManager.AddSkill(SkillFactory.Create(skillConfig,InventoryManager.Instance.PlayerSkillController));
    }
}