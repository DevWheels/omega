using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public abstract class Skill {
    public SkillConfig skillConfig { get; set; }
    public PlayerSkillController playerController { get; set; }

    public int Level { get; set; }

    public Skill(SkillConfig skillConfig, PlayerSkillController playerController) {
        this.skillConfig = skillConfig;
        this.playerController = playerController;
    }

    public abstract void Activate();


    public virtual void Upgrade() {
        Level++;
    }
    
    public abstract void Deactivate();
}