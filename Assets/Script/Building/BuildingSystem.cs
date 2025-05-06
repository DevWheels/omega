using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BuildingSystem : NetworkBehaviour
{
    public static BuildingSystem Instance;

    [SerializeField] private List<BuildingConfig> buildingConfigs;
    private Dictionary<string, BuildingConfig> buildingConfigsDict = new Dictionary<string, BuildingConfig>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var config in buildingConfigs)
        {
            buildingConfigsDict.Add(config.BuildingId, config);
        }
    }

    [Server]
    public BuildingConfig GetBuildingConfig(string buildingId)
    {
        return buildingConfigsDict.TryGetValue(buildingId, out var config) ? config : null;
    }
}

[System.Serializable]
public class BuildingConfig
{
    public string BuildingId;
    public string DisplayName;
    public GameObject BuildingPrefab;
    public GameObject BuildingUIPrefab;
    public int MaxLevel = 5;
    public List<BuildingLevel> Levels;
}

[System.Serializable]
public class BuildingLevel
{
    public int RequiredCoins;
    public List<ItemRequirement> UpgradeRequirements;
}