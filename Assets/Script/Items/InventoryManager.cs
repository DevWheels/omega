using System;
using UnityEngine;
using Mirror;

public class InventoryManager : NetworkBehaviour {
    public static InventoryManager Instance;

    [SerializeField]
    private GameObject inventorySlots;

    [SerializeField]
    private GameObject inventoryQuests;

    [field: SerializeField]
    public InventoryView InventoryView { get; private set; }

    private bool isOpened = false;

    public PlayerSkillController PlayerSkillController;

    private void Awake() {
        Instance = this;
    }

    [Client]
    public void InventoryToggle(PlayerInventory playerInventory) {
        if (!isOpened) {
            InventoryOpen(playerInventory);
        } else {
            InventoryClose();
        }
    }

    [Client]
    private void InventoryOpen(PlayerInventory playerInventory) {
        InventoryView.SetData(playerInventory);
        inventorySlots.gameObject.SetActive(true);
        inventoryQuests.gameObject.SetActive(true);
        isOpened = true;
    }

    [Client]
    private void InventoryClose() {
        inventorySlots.gameObject.SetActive(false);
        inventoryQuests.gameObject.SetActive(false);
        isOpened = false;
    }
}