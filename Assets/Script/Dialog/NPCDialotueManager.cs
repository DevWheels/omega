using UnityEngine;

public class NPCDialotueManager : MonoBehaviour
{
    public GameObject panel;
    public void Close()
    {
        panel.SetActive(false);
    }
}
