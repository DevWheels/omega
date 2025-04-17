using UnityEngine;
using System.Collections.Generic;

public class TeleportZone : MonoBehaviour
{
    public GameObject confirmationPanel;
    public List<GameObject> targetMarkers;
    public bool setGreenZone = false; // Какое состояние установить после телепортации
    
    private GameObject player; 
    private PlayerSkillController playerSkillController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player = other.gameObject;
            playerSkillController = player.GetComponent<PlayerSkillController>();
            confirmationPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            confirmationPanel.SetActive(false);
            player = null; 
            playerSkillController = null;
        }
    }

    public void ConfirmTeleport()
    {
        if (player != null && targetMarkers != null && targetMarkers.Count > 0)
        {
            int randomIndex = Random.Range(0, targetMarkers.Count);
            GameObject randomTarget = targetMarkers[randomIndex];
            
            player.transform.position = randomTarget.transform.position;
            
            if (playerSkillController != null)
            {
                playerSkillController.TeleportToArena();
            }
        }
        confirmationPanel.SetActive(false);
    }

    public void CancelTeleport()
    {
        confirmationPanel.SetActive(false); 
    }
}