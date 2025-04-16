using UnityEngine;
using TMPro;
public class ItemWorld : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI statsText;
    
    private EquipmentItem _item;

    public void SetItem(EquipmentItem item)
    {
        _item = item;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (nameText != null)
            nameText.text = _item.Config.itemName;
        
        if (statsText != null)
            statsText.text = $"Rank: {_item.Rank}\nLvl: {_item.Level}";
    }
}
