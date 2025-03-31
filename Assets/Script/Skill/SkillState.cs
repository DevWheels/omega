using System;
using UnityEngine;

public enum SkillState
{
    Locked,
    Unlocked,
    Mastered
}

[Serializable]
public class Skill
{
    public string name;
    public string description;
    public int level;
    public int maxLevel = 3;
    public SkillState state = SkillState.Locked;
    public int[] apCost; 

    public Skill(string name, string description, int maxLevel = 3)
    {
        this.name = name;
        this.description = description;
        this.maxLevel = maxLevel;
        this.apCost = new int[maxLevel];
        for (int i = 0; i < maxLevel; i++)
        {
            apCost[i] = 50 * (i + 1);
        }
    }

    public bool CanUpgrade()
    {
        return state == SkillState.Unlocked && level < maxLevel;
    }

    public bool Upgrade()
    {
        if (CanUpgrade())
        {
            level++;
            if (level == maxLevel)
            {
                state = SkillState.Mastered;
            }
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"{name} (Óð. {level}/{maxLevel}) - {description}";
    }
}