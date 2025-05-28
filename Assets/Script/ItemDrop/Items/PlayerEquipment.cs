using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    private Dictionary<ItemType, ItemConfig> PlayerInventoryConfig = new();
    private Dictionary<ItemType, ItemData> PlayerInventoryData = new();

    public static PlayerEquipment Instance;

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

    public Dictionary<ItemType, ItemConfig> GetAllItems() {
        return PlayerInventoryConfig;
    }

    public ItemConfig GetItemConfig(ItemType itemType) {
        return PlayerInventoryConfig[itemType];
    }

    public ItemData GetItemData(ItemType itemType) {
        return PlayerInventoryData[itemType];
    }
    public void WearItem(ItemConfig equipmentItemConfig,ItemData itemData) {
        
        if (equipmentItemConfig.Skills.Count < 0) {
            SetEquipmentImage(equipmentItemConfig);
            return;
        }

        var skillController = InventoryManager.Instance.PlayerSkillController;
        skillController.SkillManager.Skills.Clear();
        
        EquipmentFilter(equipmentItemConfig, itemData);

        GameUI.Instance.SkillContainerView.gameObject.GetComponent<SkillSelectorHandler>().UpdateSkillSelector();
        SetEquipmentImage(equipmentItemConfig);
    }

    private void EquipmentFilter(ItemConfig equipmentItemConfig, ItemData itemData) {
        PlayerInventoryConfig[equipmentItemConfig.itemType] = equipmentItemConfig;
        PlayerInventoryData[equipmentItemConfig.itemType] = itemData;
    }

    public void Unwear(ItemConfig equipmentItemConfig,ItemData itemData) {
        InventoryManager.Instance.PlayerSkillController.gameObject.GetComponent<PlayerInventory>().PutInEmptySlot(equipmentItemConfig,itemData);

        foreach (var skillType in itemData.Skills) {
            InventoryManager.Instance.PlayerSkillController.DeleteSkill(SkillFactory.Create(ConfigsManager.GetSkillConfig(skillType),InventoryManager.Instance.PlayerSkillController));
        }
        
    }
    private void SetEquipmentImage(ItemConfig equipmentItemConfig) {
        switch (equipmentItemConfig.itemType) {
            case ItemType.Armor:

                imageForArmor.gameObject.SetActive(true);
                imageForArmor.sprite = equipmentItemConfig.icon;
                
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
        Debug.Log(PlayerInventoryConfig[equipmentItemConfig.itemType]);
        Debug.Log(PlayerInventoryData[equipmentItemConfig.itemType]);
    
        GameUI.Instance.button.SetData(PlayerInventoryConfig[equipmentItemConfig.itemType],PlayerInventoryData[equipmentItemConfig.itemType],Unwear);
    }
}