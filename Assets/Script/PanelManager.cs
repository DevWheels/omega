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

        // Инициализация: все панели скрыты
        foreach (var panel in panels) {
            panel.SetActive(false);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (!isOpened) {
                // Показываем последнюю открытую панель
                panels[lastOpened].SetActive(true);
                isOpened = true;
            } else {
                // Скрываем все панели
                foreach (var panel in panels) {
                    panel.SetActive(false);
                }
                isOpened = false;
            }
        }
    }

    public void ShowPanel(int panelIndex) {
        for (int i = 0; i < panels.Length; i++) {
            panels[i].SetActive(i == panelIndex);
            if (i == panelIndex) {
                lastOpened = i;
            }
        }
        isOpened = true; // Устанавливаем флаг, что панель открыта
    }
}