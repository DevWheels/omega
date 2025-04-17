using UnityEngine;

[System.Serializable]
public abstract class Skill  {
    public SkillConfig skillConfig { get; set; }
    public PlayerSkillController playerController { get; set; }

    public int Level { get; set; }

    protected Skill(SkillConfig skillConfig, PlayerSkillController playerController) {
        this.skillConfig = skillConfig;
        this.playerController = playerController;
    }

    public abstract void Activate(Vector3 mousePos);


    public virtual void Upgrade() {
        Level++;
    }
    
    public abstract void Deactivate();
}