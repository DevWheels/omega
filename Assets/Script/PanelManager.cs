using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour 
{
    public GameObject[] panels;
    public Button[] buttons;
    private bool isOpened = false;
    private int lastOpened = 0;

    // Делаем менеджер одиночкой (Singleton)
    public static PanelManager Instance { get; private set; }

    private void Awake()
    {
        // Убеждаемся, что существует только один экземпляр
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start() 
    {
        // Назначаем кнопки только если это основной менеджер
        if (Instance == this)
        {
            for (int i = 0; i < buttons.Length; i++) 
            {
                int index = i;
                buttons[i].onClick.AddListener(() => ShowPanel(index));
            }

            // Инициализация: все панели скрыты
            HideAllPanels();
        }
    }

    private void Update() 
    {
        // Обрабатываем ввод только в основном менеджере
        if (Instance == this)
        {
            if (Input.GetKeyDown(KeyCode.Tab)) 
            {
                TogglePanels();
            }
            
            // Добавляем обработку Escape
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideAllPanels();
            }
        }
    }

    public void TogglePanels()
    {
        if (!isOpened) 
        {
            panels[lastOpened].SetActive(true);
            isOpened = true;
        } 
        else 
        {
            HideAllPanels();
        }
    }

    public void ShowPanel(int panelIndex) 
    {
        HideAllPanels();
        panels[panelIndex].SetActive(true);
        lastOpened = panelIndex;
        isOpened = true;
    }

    private void HideAllPanels()
    {
        foreach (var panel in panels) 
        {
            panel.SetActive(false);
        }
        isOpened = false;
    }
}