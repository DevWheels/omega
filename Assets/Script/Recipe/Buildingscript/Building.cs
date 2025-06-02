using UnityEngine;
using Mirror;
using UnityEngine.VFX;

[System.Serializable]
public class BuildingUpgradeCost
{
    public ItemConfig itemConfig;
    public int amount = 1;
}

public abstract class Building : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnBuildingLevelChanged))]
    public int buildingLevel = 1;
    
    [Header("Basic Settings")]
    [SerializeField] private string buildingName;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private GameObject craftingUIPanel; // Ссылка на UI панель
    
    [Header("Upgrade Settings")]
    [SerializeField] private BuildingUpgradeCost[] upgradeCosts;
    
    [Header("Visual Effects")]
    [SerializeField] private VisualEffect upgradeEffect;
    [SerializeField] private VisualEffect craftEffect;
    
    private bool playerInRange;

    public string BuildingName => buildingName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            playerInRange = true;
            Debug.Log("Player entered building zone");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            playerInRange = false;
            CloseUI();
            Debug.Log("Player left building zone");
        }
    }
    
    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleUI();
        }
    }
    
    private void ToggleUI()
    {
        if (craftingUIPanel == null) return;
        
        if (craftingUIPanel.activeSelf)
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }
    
    private void OpenUI()
    {
        craftingUIPanel.SetActive(true);
        SetupUI(craftingUIPanel);
        Debug.Log("Opened crafting UI");
    }
    
    public void CloseUI()
    {
        if (craftingUIPanel != null)
        {
            craftingUIPanel.SetActive(false);
            Debug.Log("Closed crafting UI");
        }
    }
    
    protected abstract void SetupUI(GameObject uiPanel);
    
    [Command(requiresAuthority = false)]
    public void CmdUpgradeBuilding(NetworkConnectionToClient sender = null)
    {
        var playerInventory = sender.identity.GetComponent<PlayerInventory>();
        if (CanUpgrade(playerInventory))
        {
            SpendUpgradeResources(playerInventory);
            buildingLevel++;
        }
    }
    
    private bool CanUpgrade(PlayerInventory inventory)
    {
        foreach (var cost in upgradeCosts)
        {
            if (!inventory.HasItem(cost.itemConfig, cost.amount))
                return false;
        }
        return true;
    }
    
    private void SpendUpgradeResources(PlayerInventory inventory)
    {
        foreach (var cost in upgradeCosts)
        {
            inventory.CmdRemoveItem(cost.itemConfig, cost.amount);
        }
    }
    
    private void OnBuildingLevelChanged(int oldLevel, int newLevel)
    {
        if (upgradeEffect != null)
        {
            upgradeEffect.Play();
        }
        
        Debug.Log($"{buildingName} upgraded to level {newLevel}");
    }
    
    public virtual void PlayCraftEffect()
    {
        if (craftEffect != null)
        {
            craftEffect.Play();
        }
    }
    
    public abstract bool CanCraft(ItemRecipe recipe, PlayerInventory inventory);
    public abstract void CraftItem(ItemRecipe recipe, PlayerInventory inventory);
}