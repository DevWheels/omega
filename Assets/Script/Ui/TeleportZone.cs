using UnityEngine;
using UnityEngine.UI;

public class TeleportZone : MonoBehaviour
{
    public GameObject confirmationPanel; 
    public GameObject targetObject; 
    private GameObject player; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player = other.gameObject;
            confirmationPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            confirmationPanel.SetActive(false);
            player = null; 
        }
    }


    public void ConfirmTeleport()
    {
        if (player != null && targetObject != null)
        {
            player.transform.position = targetObject.transform.position;
        }
        confirmationPanel.SetActive(false);
    }

    public void CancelTeleport()
    {
        confirmationPanel.SetActive(false); 
    }
}
