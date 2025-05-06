using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    private Dictionary<ItemType,EquipmentItemConfig> PlayerInventory = new();
    public static PlayerEquipment Instance;

    [SerializeField] private Image imageForArmor;
    [SerializeField] private Image imageForAccessory;
    [SerializeField] private Image imageForWeapon;

    [SerializeField] private Image skillSelectorPanel;
    private void Awake() {
        Instance = this;

    }

    public Dictionary<ItemType, EquipmentItemConfig> GetAllItems() {
        return PlayerInventory;
    }
    public void WearItem(EquipmentItemConfig equipmentItemConfig) {
        if (equipmentItemConfig.itemSkills.Count < 0) {
            SetEquipmentImage(equipmentItemConfig);
            return;
        }
        var skillController = InventoryManager.Instance.PlayerSkillController;
        skillController.SkillManager.Skills.Clear();
        

        if (PlayerInventory.ContainsKey(equipmentItemConfig.itemType)) {

                PlayerInventory[equipmentItemConfig.itemType] = equipmentItemConfig;
                
        }
        else {

                PlayerInventory.Add(equipmentItemConfig.itemType, equipmentItemConfig);
        }

        skillController.gameObject.GetComponent<SkillSelectorHandler>().UpdateSkillSelector();
        // skillController.AddNewSkillFromItem();
        SetEquipmentImage(equipmentItemConfig);
    }

    private void SetEquipmentImage(EquipmentItemConfig equipmentItemConfig) {
        switch (equipmentItemConfig.itemType) {
            case ItemType.Armor: 

                imageForArmor.gameObject.SetActive(true);
                imageForArmor.sprite = equipmentItemConfig.icon; break;
            case ItemType.Accessory:

                imageForAccessory.gameObject.SetActive(true);
                imageForAccessory.sprite = equipmentItemConfig.icon; break;
            case ItemType.Weapon:
                
                imageForWeapon.gameObject.SetActive(true);
                imageForWeapon.sprite = equipmentItemConfig.icon; break;
            default: Debug.LogError("not correct item type or no type"); break;
        }
    }


}