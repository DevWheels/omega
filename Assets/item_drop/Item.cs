using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Iteme
{
    public string Name1;
    public ItemRank Rank;
    public int Level;
    public int Health;
    public int Armor;
    public int Attack;
    public List<SpecialStat> SpecialStats;

    public Iteme(string name, ItemRank rank, int level, int health, int armor, int attack, List<SpecialStat> specialStats)
    {
        Name1 = name;
        Rank = rank;
        Level = level;
        Health = health;
        Armor = armor;
        Attack = attack;
        SpecialStats = specialStats;
    }

    public void PrintItemInfo()
    {
        Debug.Log($"Item: {Name1}");
        Debug.Log($"Rank: {Rank}, Level: {Level}");
        Debug.Log($"Stats: Health - {Health}, Armor - {Armor}, Attack - {Attack}");
        Debug.Log("Special Stats:");
        foreach (var stat in SpecialStats)
        {
            Debug.Log($"- {stat}");
        }
    }
}