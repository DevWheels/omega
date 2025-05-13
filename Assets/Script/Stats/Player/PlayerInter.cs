using Mirror;
using UnityEngine;

public class PlayerInter : NetworkBehaviour
{
    [SyncVar] private ChestInteraction _chest;
    [SyncVar] private PlayerStats _playerStats;
    
    private PlayerStats _localPlayerStats; // Локальная ссылка для клиента
    
    [Command]
    public void CmdTryOpenChest() 
    {
        if (_playerStats != null)
        {
            _chest.TryOpenChest(_playerStats.Lvl);
        }
    }

    public void Update() 
    {
        if (isLocalPlayer && _chest != null && Input.GetKeyDown(KeyCode.E))
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
        if (other.CompareTag("Chest") && _chest == other.GetComponent<ChestInteraction>()) 
        {
            _chest = null;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        // Инициализация на сервере после спавна
        _playerStats = GetComponent<PlayerStats>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // Для локального использования на клиенте
        _localPlayerStats = GetComponent<PlayerStats>();
        
        // Если мы не сервер, но хотим иметь доступ к PlayerStats
        if (!isServer && _playerStats == null)
        {
            _playerStats = _localPlayerStats;
        }
    }
}