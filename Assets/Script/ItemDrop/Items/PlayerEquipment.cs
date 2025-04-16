using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerEquipment : NetworkBehaviour {
        public List<EquipmentItemConfig> PlayerInventory;
        public static PlayerEquipment Instance;

        private void Awake() {
                Instance = this;
        }

        public List<EquipmentItemConfig> GetPlayerInventory() {
                return PlayerInventory;
        }
}
