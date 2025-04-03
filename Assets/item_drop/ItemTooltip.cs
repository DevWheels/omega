using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class ItemTooltip : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 1, 0);
    [SerializeField] private float showDelay = 0.5f;
    [Header("References")]
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemRankText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text armorText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text specialStatsText;

    [Header("Settings")]
    [SerializeField] private bool debugMode = true;

    private Iteme currentItem;

    public void ShowTooltip(Iteme item)
    {
        if (debugMode) Debug.Log($"[Tooltip] Attempting to show tooltip for {item?.Name1}");

        if (item == null)
        {
            if (debugMode) Debug.LogWarning("[Tooltip] Null item received");
            return;
        }

        currentItem = item;

        if (tooltipPanel == null)
        {
            Debug.LogError("[Tooltip] Tooltip panel reference is null!");
            return;
        }

        UpdateTooltipUI();
        tooltipPanel.SetActive(true);

        if (debugMode) Debug.Log($"[Tooltip] Tooltip shown for {item.Name1}");
    }

    private void UpdateTooltipUI()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("[Tooltip] Current item is null during UI update");
            return;
        }

        try
        {
  
            itemNameText?.SetText(currentItem.Name1 ?? "No Name");
            itemRankText?.SetText($"Rank: {currentItem.Rank.ToString()}");
            healthText?.SetText($"HP: {currentItem.Health.ToString()}");
            armorText?.SetText($"Armor: {currentItem.Armor.ToString()}");
            attackText?.SetText($"Attack: {currentItem.Attack.ToString()}");
            specialStatsText?.SetText($"SpecialStats: {currentItem.SpecialStats.ToString()}");

            if (debugMode) Debug.Log("[Tooltip] UI updated successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Tooltip] UI update failed: {e.Message}");
        }
    }

    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }
}


