using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    private Dictionary<ItemType,EquipmentItemConfig> PlayerInventory = new();
    public static PlayerEquipment Instance;

    [SerializeField] private Image _imageForArmor;
    [SerializeField] private Image _imageForAccessory;
    [SerializeField] private Image _imageForWeapon;

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
        
        // foreach (var skillIndex in PlayerInventory) {
        //     foreach (var itemskill in skillIndex.Value.itemSkills) {
        //         var skill = SkillFactory.Create(itemskill, skillController);
        //         skillController.AddNewSkillFromItem();                
        //     }
        //
        // }
        skillController.AddNewSkillFromItem();
        SetEquipmentImage(equipmentItemConfig);
    }

    private void SetEquipmentImage(EquipmentItemConfig equipmentItemConfig) {
        switch (equipmentItemConfig.itemType) {
            case ItemType.Armor: 

                _imageForArmor.gameObject.SetActive(true);
                _imageForArmor.sprite = equipmentItemConfig.icon; break;
            case ItemType.Accessory:

                _imageForAccessory.gameObject.SetActive(true);
                _imageForAccessory.sprite = equipmentItemConfig.icon; break;
            case ItemType.Weapon:
                
                _imageForWeapon.gameObject.SetActive(true);
                _imageForWeapon.sprite = equipmentItemConfig.icon; break;
            default: Debug.LogError("not correct item type or no type"); break;
        }
    }


}