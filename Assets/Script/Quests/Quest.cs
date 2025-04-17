using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest {
    [Header("NPC_Text")] public string title;
    public string description;
    public string question;
    public string thanks;
    [Header("Rewards")]
    public int experience;
    public List<ItemData> reward_items_data;
    public List<ItemConfig> reward_items_config;
    public List<ItemConfig> quest_items_config;
    public List<ItemData> quest_items_data;
    public bool isCompleted;
    public bool isStarted;

    //public Quest(string title, string description, string question, string thanks, int experience, List<Item> items, List<Item> quest_items)
    //{
    //    this.title = title;
    //    this.description = description;
    //    this.question = question;
    //    this.thanks = thanks;
    //    this.experience = experience;

    //    this.reward_items = items;
    //    this.quest_items = quest_items;

    //    this.isCompleted = false;
    //    this.isStarted = false;
    //}


    public void CompleteQuest() {
        isCompleted = true;
        for (var index = 0; index < reward_items_data.Count; index++) {
            var item = reward_items_data[index];
            if (item != null) {
                InventoryView.instance.PutInEmptySlot(reward_items_config[index],item);
            }
        }
    }
    public void SaveQuest()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("quest_" + this.title, json);
        PlayerPrefs.Save();
    }
}