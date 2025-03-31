using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillLevelText;
    public TextMeshProUGUI skillDescriptionText;
    public Button upgradeButton;
    public TextMeshProUGUI apCostText;

    private Skill currentSkill;
    private Player1 player;

    public void Setup(Skill skill, Player1 player)
    {
        currentSkill = skill;
        this.player = player;

        skillNameText.text = skill.name;
        skillLevelText.text = $"Óð. {skill.level}/{skill.maxLevel}";
        skillDescriptionText.text = skill.description;

        UpdateButton();
    }

    private void UpdateButton()
    {
        if (currentSkill.CanUpgrade() && player.ap >= currentSkill.apCost[currentSkill.level])
        {
            upgradeButton.interactable = true;
            apCostText.text = currentSkill.apCost[currentSkill.level].ToString();
        }
        else
        {
            upgradeButton.interactable = false;
            apCostText.text = "MAX";
        }
    }

    public void OnUpgradeButton()
    {
        if (player.UpgradeSkill(currentSkill))
        {
            UpdateButton();
        }
    }
}
