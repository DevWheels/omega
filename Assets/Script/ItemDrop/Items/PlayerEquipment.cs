using System;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipment : NetworkBehaviour {
        public List<EquipmentItemConfig> PlayerInventory;
        public static PlayerEquipment Instance;
        private GameObject _panel;
        private GameObject _imageForArmor;
        private void Awake() {
                Instance = this;
        }

        private void Start() {
                _panel = GameObject.Find("Left");
                _imageForArmor = _panel.transform.GetChild(0).transform.GetChild(0).gameObject;
                // Debug.Log(_panel.transform.GetChild(0));
                // Debug.Log(_panel.transform.GetChild(0).transform.GetChild(0).GetComponent<UnityEngine.UI.Image>());

        }

        public List<EquipmentItemConfig> GetPlayerInventory() {
                return PlayerInventory;
        }

        public void WearItem(EquipmentItemConfig equipmentItemConfig) {
                PlayerInventory.Add(equipmentItemConfig);
                SetEquipmentImage(equipmentItemConfig);
                
                if (equipmentItemConfig.itemSkills.Count < 0) {
                        return;
                }
                var skillController = Inventory.instance.Player.GetComponent<PlayerSkillController>();
                
                foreach (var skillIndex in equipmentItemConfig.itemSkills) {
                        var skill = SkillFactory.Create(skillIndex, skillController);
                        skillController.AddNewSkillFromItem(skill);
                }
                

        }

        private void SetEquipmentImage(EquipmentItemConfig equipmentItemConfig) {
                _imageForArmor.GetComponent<UnityEngine.UI.Image>().sprite = equipmentItemConfig.icon;
        }
}
