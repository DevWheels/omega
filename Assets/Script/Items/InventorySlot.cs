using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [FormerlySerializedAs("SlotItem")] public ItemConfig slotItemConfig;
    public ItemData slotItemData;
    Image icon;
    Button button;

    private void Start()
    {
        icon = gameObject.transform.GetChild(0).GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowInfo);
    }
    public void PutInSlot(ItemConfig itemConfig,ItemData itemData)
    {
        
        icon.sprite = itemConfig.icon;
        slotItemConfig = itemConfig;
        slotItemData = itemData;
        icon.enabled = true;
    }

    void ShowInfo()
    {
        if (slotItemConfig != null)
            ItemInfo.instance.Open(slotItemConfig,slotItemData, this);
        
    }

    public void ClearSlot()
    {
        slotItemConfig = null;
        slotItemData = null;
        icon.sprite = null;
        icon.enabled = false;
    }




}
 
       

