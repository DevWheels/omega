using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerEquipment : NetworkBehaviour {
    private Dictionary<ItemType, ItemConfig> PlayerInventory = new();
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
    [SerializeField] private PlayerStats playerStats;
    
    [SerializeField] private UnwearItemButton UnwearButton;

    private void Awake() {
        Instance = this;
    }

    public Dictionary<ItemType, ItemConfig> GetAllItems() {
        return PlayerInventory;
    }

    public void WearItem(ItemConfig equipmentItemConfig, ItemData itemData)
    {
        _itemData = itemData;
        ApplyItemStats(equipmentItemConfig);

        if (equipmentItemConfig.itemSkills.Count < 0)
        {
            SetEquipmentImage(equipmentItemConfig);
            return;
        }

        var skillController = InventoryManager.Instance.PlayerSkillController;
        skillController.SkillManager.Skills.Clear();

        if (PlayerInventory.ContainsKey(equipmentItemConfig.itemType))
        {

            RemoveItemStats(PlayerInventory[equipmentItemConfig.itemType]);
            PlayerInventory[equipmentItemConfig.itemType] = equipmentItemConfig;
        }
        else
        {
            PlayerInventory.Add(equipmentItemConfig.itemType, equipmentItemConfig);
        }

        GameUI.Instance.SkillContainerView.gameObject.GetComponent<SkillSelectorHandler>().UpdateSkillSelector();
        SetEquipmentImage(equipmentItemConfig);
    }

    private void Unwear(ItemConfig equipmentItemConfig, ItemData itemData, List<SkillConfig> skillConfig)
    {

        RemoveItemStats(equipmentItemConfig);
    
        InventoryManager.Instance.PlayerSkillController.gameObject.GetComponent<PlayerInventory>().PutInEmptySlot(equipmentItemConfig, itemData);

        foreach (var skillConf in skillConfig)
        {
            InventoryManager.Instance.PlayerSkillController.DeleteSkill(SkillFactory.Create(skillConf, InventoryManager.Instance.PlayerSkillController));
        }
    }
    private void RemoveItemStats(ItemConfig itemConfig)
    {
        var playerStats = GetComponent<PlayerStats>();
        if (playerStats == null) return;

        if (itemConfig is EquipmentItemData equipmentData)
        {
            playerStats.MaxHp -= equipmentData.Health;
            playerStats.Armor -= equipmentData.Armor;
            
            for (int i = 0; i < equipmentData.SpecialStats.Length; i++)
            {
               
            }
        }
    }
    private void ApplyItemStats(ItemConfig itemConfig)
    {
        var playerStats = GetComponent<PlayerStats>();
        if (playerStats == null) return;
        if (itemConfig is EquipmentItemData equipmentData)
        {
            playerStats.MaxHp += equipmentData.Health;
            playerStats.Armor += equipmentData.Armor;
            for (int i = 0; i < equipmentData.SpecialStats.Length; i++)
            {
                // Здесь можно добавить логику для специальных характеристик
                // Например, увеличение крит. шанса, уклонения и т.д.
            }
        }
    }
    
    private void SetEquipmentImage(ItemConfig equipmentItemConfig) {
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