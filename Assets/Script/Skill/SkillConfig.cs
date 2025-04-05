using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill")]
public class SkillConfig : ScriptableObject {
    public string Name;
    public string Description;
    
    public Transform SpawnPoint;
    public GameObject ProjectilePrefab;

    public Sprite Icon;
    public float Cooldown { get; set; }
    public int ManaCost { get; set; }
    public int Level { get; set; }
    public bool IsPassive { get; set; }

}