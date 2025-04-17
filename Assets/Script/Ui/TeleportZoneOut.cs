using UnityEngine;
using System.Collections.Generic;

public class TeleportZoneOut : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject targetMarker;  // Теперь это один конкретный маркер
    public bool setGreenZone = false; // Какое состояние установить после телепортации
    
    private GameObject player; 
    private PlayerSkillController playerSkillController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player = other.gameObject;
            var ps = player.GetComponent<PlayerSkillController>();
            if (ps.Playermovement.isLocalPlayer) {
                playerSkillController = ps;
                confirmationPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerSkillController == null) {
            return;
        }

        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PlayerSkillController>() == playerSkillController) {
                confirmationPanel.SetActive(false);
                player = null; 
                playerSkillController = null;
            }
        }
    }

    public void ConfirmTeleport()
    {
        if (player != null && targetMarker != null)
        {
            player.transform.position = targetMarker.transform.position;
            
            if (playerSkillController != null)
            {
                playerSkillController.TeleportToGreenZone();
            }
        }
        confirmationPanel.SetActive(false);
    }

    public void CancelTeleport()
    {
        confirmationPanel.SetActive(false); 
    }
}