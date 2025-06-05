using UnityEngine;
using UnityEngine.UI;

public class EscapePanelController : MonoBehaviour
{
    public GameObject escapePanel; 
    public GameObject otherUI; 

    private void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (otherUI != null && !otherUI.activeInHierarchy)
            {
                ToggleEscapePanel();
            }
        }
    }

    // Переключаем видимость панели ESC
    private void ToggleEscapePanel()
    {
        bool newState = !escapePanel.activeSelf;
        escapePanel.SetActive(newState);
        
        if (newState)
        {
            Debug.Log("Escape panel opened");
        }
        else
        {
    
            Debug.Log("Escape panel closed");
        }
    }

  
    public void OnContinueButton()
    {
        escapePanel.SetActive(false);
    }
    
    public void OnQuitButton()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}