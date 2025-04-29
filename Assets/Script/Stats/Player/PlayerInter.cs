using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInter : NetworkBehaviour
{
    [SyncVar] private ChestInteraction _chest;
    [SyncVar] private PlayerStats _playerStats;
    [Command]
    
    public void CmdTryOpenChest() {
        _chest.TryOpenChest(_playerStats.Lvl);
    }

    public void Update() {
        if (_chest!=null && Input.GetKeyDown(KeyCode.E))
        {
            CmdTryOpenChest();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Chest")) return;
        
        _chest = other.GetComponent<ChestInteraction>();
        
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Chest") && _chest == other.GetComponent<ChestInteraction>()) {
            _chest = null;
        }
    }

    private void Awake() {
        _playerStats = GetComponent<PlayerStats>();
    }
}
