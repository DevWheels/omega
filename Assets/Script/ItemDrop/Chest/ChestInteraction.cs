using System;
using Mirror;
using UnityEngine;

public class ChestInteraction : NetworkBehaviour{
    [SyncVar] private bool _isNearby = false;
    [SyncVar] private PlayerStats _playerStats;
    
    private void Update() {
        if (_isNearby && Input.GetKeyDown(KeyCode.E)) {
            GetComponent<EnemyLoot>().DropItem(_playerStats.Lvl);
            
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        _isNearby = true;
        if (_playerStats.TryGetComponent<PlayerStats>(out PlayerStats player)) {
            _playerStats = player;
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        _isNearby = false;
    }
}
