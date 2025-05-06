using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerBuildings : NetworkBehaviour
{
    [SyncVar] private Dictionary<string, int> playerBuildings = new Dictionary<string, int>();

    public int GetBuildingLevel(string buildingId)
    {
        return playerBuildings.TryGetValue(buildingId, out var level) ? level : 0;
    }

    [Command]
    public void CmdUpgradeBuilding(string buildingId)
    {
        var currentLevel = GetBuildingLevel(buildingId);
        var buildingConfig = BuildingSystem.Instance.GetBuildingConfig(buildingId);

        if (buildingConfig == null || currentLevel >= buildingConfig.MaxLevel) return;

        var nextLevel = currentLevel + 1;
        var levelRequirements = buildingConfig.Levels[nextLevel - 1];


        var inventory = GetComponent<PlayerInventory>();
        var coins = 0; 
        
        if (coins < levelRequirements.RequiredCoins) return;

        foreach (var requirement in levelRequirements.UpgradeRequirements)
        {
            if (!inventory.HasItems(requirement.ItemType, requirement.Amount))
            {
                return;
            }
        }

        foreach (var requirement in levelRequirements.UpgradeRequirements)
        {
            inventory.RemoveItems(requirement.ItemType, requirement.Amount);
        }

        playerBuildings[buildingId] = nextLevel;
    }
}