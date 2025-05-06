using Mirror;
using UnityEngine;

public class PlayerInter : NetworkBehaviour {
    private ChestInteraction _chest;
    private PlayerStats _playerStats;

    private void Awake() {
        _playerStats = GetComponent<PlayerStats>();
    }

    public void Update() {
        if (_chest != null && Input.GetKeyDown(KeyCode.E)) {
            CmdTryOpenChest();
        }
    }

    [Command]
    private void CmdTryOpenChest() {
        _chest.TryOpenChest(_playerStats.Lvl);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Chest")) {
            return;
        }

        _chest = other.GetComponent<ChestInteraction>();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.CompareTag("Chest")) {
            return;
        }

        if (_chest == other.GetComponent<ChestInteraction>()) {
            _chest = null;
        }
    }
}