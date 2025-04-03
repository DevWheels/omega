using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillConfig : ScriptableObject {
    public string Name;
    public string Description;
    
    
    public GameObject ProjectilePrefab;

    public Sprite Icon;


}