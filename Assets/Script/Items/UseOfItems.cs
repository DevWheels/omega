using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseOfItems : MonoBehaviour
{
    public static UseOfItems instance;
    private PlayerStats playerStats; 

    private void Start()
    {
        instance = this;
        playerStats = FindObjectOfType<PlayerStats>(); 
    }

    public void Use(ItemConfig itemConfig)
    {
        if (itemConfig.isHealing)
        {
            Debug.Log("�� ������������ �������� �� " + itemConfig.HealingPower);
            playerStats.UseItem(itemConfig); 
        }

        if (itemConfig.isMana)
        {
            Debug.Log("�� ������������ ���� �� " + itemConfig.ManaPower);
            playerStats.UseItem(itemConfig); 
        }
    }
}
