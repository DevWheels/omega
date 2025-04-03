using UnityEngine;
using UnityEngine.UIElements;


public class Skill : MonoBehaviour
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Cooldown { get; set; }
    public int ManaCost { get; set; }
    public int Level { get; set; }
    public Sprite Icon { get; set; }
    public bool IsPassive  { get; set; }

    public virtual void Activate() {
        //Реализация активации навыка
        
    }

    public virtual void Upgrade() {
        //Реализация улучшения навыка
        Level++;
    }
}
