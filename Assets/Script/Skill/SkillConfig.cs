using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillConfig : ScriptableObject {
    public string Name;
    [TextArea]
    public string Description;
    public bool isPassive;

    [Header("Set if it's passive skill")]
    public int PercenageBuff;
    [Header("Set if it's active skill")]
    public int Damage;

    

    public ProjectileBase ProjectilePrefab;

    public Sprite Icon;


}