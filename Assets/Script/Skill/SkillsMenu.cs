using System.Collections.Generic;
using UnityEngine;

public class SkillsMenu : MonoBehaviour
{
    public GameObject skillSlotPrefab;
    public Transform skillsContainer;
    public Player1 player;

    private List<GameObject> skillSlots = new List<GameObject>();

    void Start()
    {
        UpdateSkillsDisplay();
    }

    public void UpdateSkillsDisplay()
    {
  
        foreach (var slot in skillSlots)
        {
            Destroy(slot);
        }
        skillSlots.Clear();


        foreach (var skill in player.availableSkills)
        {
            GameObject skillSlot = Instantiate(skillSlotPrefab, skillsContainer);
            skillSlots.Add(skillSlot);


            SkillSlot slotComponent = skillSlot.GetComponent<SkillSlot>();
            if (slotComponent != null)
            {
                slotComponent.Setup(skill, player);
            }
        }

 
    }
}
