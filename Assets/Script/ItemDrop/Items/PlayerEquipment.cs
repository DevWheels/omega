using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;

public class PlayerEquipment : NetworkBehaviour {
        public List<EquipmentItemConfig> PlayerInventory;
        public static PlayerEquipment Instance;

        private void Awake() {
                Instance = this;
        }

        public List<EquipmentItemConfig> GetPlayerInventory() {
                return PlayerInventory;
        }

        public void WearItem(EquipmentItemConfig equipmentItemConfig) {
                PlayerInventory.Add(equipmentItemConfig);
                var skillController = Inventory.instance.Player.GetComponent<PlayerSkillController>();

                foreach (var skillIndex in equipmentItemConfig.itemSkills) {
                        var skill = SkillFactory.Create(skillIndex, skillController);
                        skillController.AddNewSkillFromItem(skill);
                }
                

        }
}
