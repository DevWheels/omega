using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill {
    public SkillConfig skillConfig { get; set; }

    

    //Реализация активации навыка
    public abstract void Activate();


    public abstract void Upgrade();
    
    public abstract void Deactivate();
}