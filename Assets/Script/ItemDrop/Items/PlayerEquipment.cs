using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    public List<EquipmentItemConfig> PlayerInventory;
    public static PlayerEquipment Instance;

    [SerializeField]
    private Image _imageForArmor;

    private void Awake() {
        Instance = this;
    }

    public List<EquipmentItemConfig> GetPlayerInventory() {
        return PlayerInventory;
    }

    public void WearItem(EquipmentItemConfig equipmentItemConfig) {
        if (equipmentItemConfig.itemSkills.Count < 0) {
            return;
        }

        var skillController = InventoryManager.Instance.PlayerSkillController;
        var resSkills = new List<SkillConfig>();
        foreach (var skillIndex in equipmentItemConfig.itemSkills) {
            resSkills.Add(skillIndex);
        }

        resSkills = resSkills.DistinctBy(s => s.Name).ToList();

        foreach (var skillIndex in resSkills) {
            var skill = SkillFactory.Create(skillIndex, skillController);
            skillController.AddNewSkillFromItem(skill);

            PlayerInventory.Add(equipmentItemConfig);
        }

        PlayerInventory = PlayerInventory.DistinctBy(s => s.Name).ToList();

        SetEquipmentImage(equipmentItemConfig);
    }

    private void SetEquipmentImage(EquipmentItemConfig equipmentItemConfig) {
        _imageForArmor.gameObject.SetActive(true);
        _imageForArmor.sprite = equipmentItemConfig.icon;
    }
}