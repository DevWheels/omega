using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Quest {
    [Header("NPC_Text")] public string title;
    public string description;
    public string question;
    public string thanks;
    [Header("Rewards")] public int experience;
    public List<Item> reward_items;
    public List<Item> quest_items;

    public bool isCompleted;
    public bool isStarted;

    public void CompleteQuest() {
        isCompleted = true;
        foreach (var item in reward_items) {
            if (item != null) {
                Inventory.instance.PutInEmptySlot(item, null);
            }
        }
    }

    public void SaveQuest() {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("quest_" + this.title, json);
        PlayerPrefs.Save();
    }
}