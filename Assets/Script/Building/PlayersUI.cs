using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
public class PlayersUI : MonoBehaviour
{
    public static PlayersUI Instance;

    [Header("Interaction")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TMP_Text interactionText;

    [Header("Building UI")]
    [SerializeField] private GameObject buildingUIPanel;
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text buildingLevelText;
    [SerializeField] private Transform recipesContainer;
    [SerializeField] private GameObject recipePrefab;
    [SerializeField] private Button upgradeButton;

    private BuildingConfig currentBuildingConfig;
    private int currentBuildingLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;
        interactionPrompt.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        interactionPrompt.SetActive(false);
    }

    public void ShowBuildingUI(BuildingConfig config, int level)
    {
        currentBuildingConfig = config;
        currentBuildingLevel = level;

        buildingNameText.text = config.DisplayName;
        buildingLevelText.text = $"Level: {level}/{config.MaxLevel}";
        

        foreach (Transform child in recipesContainer)
        {
            Destroy(child.gameObject);
        }


        if (config.BuildingId == "alchemy")
        {
            var alchemyBuilding = FindObjectOfType<AlchemyBuilding>();
            foreach (var recipe in alchemyBuilding.Recipes)
            {
                if (recipe.RequiredBuildingLevel > level) continue;
                
                var recipeUI = Instantiate(recipePrefab, recipesContainer);
                recipeUI.GetComponent<RecipeUI>().Initialize(recipe);
            }
        }


        upgradeButton.interactable = level < config.MaxLevel;
        buildingUIPanel.SetActive(true);
    }

    public void HideBuildingUI()
    {
        buildingUIPanel.SetActive(false);
    }

    public void OnUpgradeButtonClick()
    {
        var player = NetworkClient.localPlayer.GetComponent<PlayerBuildings>();
        player.CmdUpgradeBuilding(currentBuildingConfig.BuildingId);
        HideBuildingUI();
    }
}