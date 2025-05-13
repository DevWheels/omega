using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    private Dictionary<ItemType, EquipmentItemConfig> PlayerInventory = new();
    public static PlayerEquipment Instance;
    private ItemConfig _itemConfig;
    private ItemData _itemData;

    [SerializeField] private Image imageForArmor;
    [SerializeField] private Image imageForAccessory;
    [SerializeField] private Image imageForWeapon;
    [SerializeField] private Image imageForBoots;
    [SerializeField] private Image imageForHelmet;
    [SerializeField] private Image imageForShoulderPads;
    [SerializeField] private Image imageForChestPlate;
    [SerializeField] private Image imageForBracers;
    [SerializeField] private Image imageForNecklace;
    [SerializeField] private Image imageForBracelet;

    
    [SerializeField] private UnwearItemButton UnwearButton;

    private void Awake() {
        Instance = this;
    }

    public Dictionary<ItemType, EquipmentItemConfig> GetAllItems() {
        return PlayerInventory;
    }

    public void WearItem(EquipmentItemConfig equipmentItemConfig,ItemConfig itemConfig,ItemData itemData) {

        _itemConfig = itemConfig;
        _itemData = itemData;
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

        GameUI.Instance.SkillContainerView.gameObject.GetComponent<SkillSelectorHandler>().UpdateSkillSelector();
        SetEquipmentImage(equipmentItemConfig);
    }

    private void Unwear(ItemConfig itemConfig,ItemData itemData,List<SkillConfig> skillConfig) {
        InventoryManager.Instance.PlayerSkillController.gameObject.GetComponent<PlayerInventory>().PutInEmptySlot(itemConfig,itemData);

        foreach (var skillConf in skillConfig) {
            InventoryManager.Instance.PlayerSkillController.DeleteSkill(SkillFactory.Create(skillConf,InventoryManager.Instance.PlayerSkillController));
        }
        
    }
    private void SetEquipmentImage(EquipmentItemConfig equipmentItemConfig) {
        switch (equipmentItemConfig.itemType) {
            case ItemType.Armor:

                imageForArmor.gameObject.SetActive(true);
                imageForArmor.sprite = equipmentItemConfig.icon;
                
                // UnwearButton.SetData(equipmentItemConfig.itemSkills,_itemConfig,_itemData,Unwear);
                break;
            case ItemType.Accessory:

                imageForAccessory.gameObject.SetActive(true);
                imageForAccessory.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.Weapon:

                imageForWeapon.gameObject.SetActive(true);
                imageForWeapon.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.weapon:
                imageForWeapon.gameObject.SetActive(true);
                imageForWeapon.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.boots:

                imageForBoots.gameObject.SetActive(true);
                imageForBoots.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.bracelet:
                imageForBracelet.gameObject.SetActive(true);
                imageForBracelet.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.chestPlate:
                imageForChestPlate.gameObject.SetActive(true);
                imageForChestPlate.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.necklace:
                imageForNecklace.gameObject.SetActive(true);
                imageForNecklace.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.shoulderPads:
                imageForShoulderPads.gameObject.SetActive(true);
                imageForShoulderPads.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.bracers:
                imageForBracers.gameObject.SetActive(true);
                imageForBracers.sprite = equipmentItemConfig.icon;
                break;
            case ItemType.helmet:
                imageForHelmet.gameObject.SetActive(true);
                imageForHelmet.sprite = equipmentItemConfig.icon;
                break;
            default:
                Debug.LogError("not correct item type or no type: " + equipmentItemConfig.itemType);
                break;
        }
    }
}