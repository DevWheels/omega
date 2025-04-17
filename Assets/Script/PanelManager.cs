using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour {
    public GameObject[] panels;
    public Button[] buttons;
    private bool isOpened = false;
    private int lastOpened;

    void Start() {
        for (int i = 0; i < buttons.Length; i++) {
            int index = i;
            buttons[i].onClick.AddListener(() => ShowPanel(index));
        }

        //ShowPanel(0);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (!isOpened) {
                panels[lastOpened].gameObject.SetActive(true);
                isOpened = true;
            } else {
                panels[lastOpened].gameObject.SetActive(false);
                isOpened = false;
            }
        }
    }

    public void ShowPanel(int panelIndex) {
        for (int i = 0; i < panels.Length; i++) {
            if (i == panelIndex) {
                panels[i].gameObject.SetActive(true);
                lastOpened = i;
            } else {
                panels[i].gameObject.SetActive(false);
            }
            //panels[i].SetActive(i == panelIndex); 
        }
    }
}