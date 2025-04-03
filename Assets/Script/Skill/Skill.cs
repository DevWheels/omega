using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill {
    public SkillConfig skillConfig { get; set; }

    public float Cooldown { get; set; }
    public int ManaCost { get; set; }
    public int Level { get; set; }
    public bool IsPassive { get; set; }

    //Реализация активации навыка
    public abstract void Activate();


    public abstract void Upgrade();
}