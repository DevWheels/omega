using UnityEngine;

public class PanelTrigger : MonoBehaviour
{
    public GameObject panel; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(true);
            Debug.Log("Панель включена!");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(false);
            Debug.Log("Панель выключена!"); 
        }
    }
}