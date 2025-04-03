using UnityEngine;

public class PlayerSkillController : MonoBehaviour {
    public SkillManager SkillManager { get; private set; }
    public SkillTree SkillTree { get; private set; }

    private void Awake() {
        SkillManager = gameObject.AddComponent<SkillManager>();

        SkillTree = gameObject.AddComponent<SkillTree>();

        //Дальше добавление навыков
    }
}