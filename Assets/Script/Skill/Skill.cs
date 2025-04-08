using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill {
    public SkillConfig skillConfig { get; set; }

    public float Cooldown { get; set; }
    public int ManaCost { get; set; }
    public int Level { get; set; }
    public bool IsPassive { get; set; }


    public abstract void Activate();


    public virtual void Upgrade() {
        Level++;
    }
    
    public abstract void Deactivate();
}