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
    
    [SerializeField] private string buildingName;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private GameObject uiPanelPrefab;
    [SerializeField] private BuildingUpgradeCost[] upgradeCosts;
    
    [Header("Visual Effects")]
    [SerializeField] private VisualEffect upgradeEffect;
    [SerializeField] private VisualEffect craftEffect;
    
    private GameObject currentUIPanel;
    private bool playerInRange;
    
    public string BuildingName => buildingName;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            playerInRange = true;
            PlayerInputHandler.Instance.OnInteractKeyPressed += TryOpenBuildingUI;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        var player = other.GetComponent<NetworkIdentity>();
        if (player.isLocalPlayer)
        {
            playerInRange = false;
            PlayerInputHandler.Instance.OnInteractKeyPressed -= TryOpenBuildingUI;
            CloseUI();
        }
    }
    
    public void TryOpenBuildingUI()
    {
        if (currentUIPanel == null)
        {
            currentUIPanel = Instantiate(uiPanelPrefab, UICanvas.Instance.transform);
            SetupUI(currentUIPanel);
        }
        else
        {
            CloseUI();
        }
    }
    
    protected abstract void SetupUI(GameObject uiPanel);
    
    public void CloseUI()
    {
        if (currentUIPanel != null)
            Destroy(currentUIPanel);
    }
    
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
        if (inventory == null) return false;
    
        foreach (var cost in upgradeCosts)
        {
            if (cost.itemConfig == null)
            {
                Debug.LogWarning("Upgrade cost itemConfig is null!");
                continue;
            }
        
            if (!inventory.HasItem(cost.itemConfig.name, cost.amount))
            {
                Debug.Log($"Missing upgrade item: {cost.itemConfig.name} x{cost.amount}");
                return false;
            }
        }
        return true;
    }

    private void SpendUpgradeResources(PlayerInventory inventory)
    {
        foreach (var cost in upgradeCosts)
        {
            inventory.CmdRemoveItemSimple(cost.itemConfig.name, cost.amount);
        }
    }
    
    private void OnBuildingLevelChanged(int oldLevel, int newLevel)
    {
        if (upgradeEffect != null)
        {
            upgradeEffect.Play();
        }
        
        Debug.Log($"{buildingName} улучшено до уровня {newLevel}");
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