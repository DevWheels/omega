using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill {
    public SkillConfig skillConfig { get; set; }
    public int Level { get; set; }
    

    public abstract void Activate();


    public virtual void Upgrade() {
        Level++;
    }
    
    public abstract void Deactivate();
}