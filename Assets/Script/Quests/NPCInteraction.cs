using UnityEngine;
using UnityEngine.UI; // Для работы с UI

[RequireComponent(typeof(Collider))] // Обязательно добавляет коллайдер
public class NPCInteraction : MonoBehaviour
{
    [Header("Настройки UI")]
    [SerializeField] private Button interactionButton; // Сама кнопка (не GameObject)
    [SerializeField] private GameObject dialogPanel;   // Панель диалога

    private void Start()
    {
        // Настраиваем коллайдер
        GetComponent<Collider>().isTrigger = true;

        // Скрываем UI при старте
        if (interactionButton != null) 
        {
            interactionButton.gameObject.SetActive(false);
            // Назначаем действие при нажатии кнопки
            interactionButton.onClick.AddListener(ToggleDialog);
        }
        
        if (dialogPanel != null) 
            dialogPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionButton != null)
        {
            interactionButton.gameObject.SetActive(true); // Показываем кнопку
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (interactionButton != null) 
                interactionButton.gameObject.SetActive(false); // Скрываем кнопку
            
            if (dialogPanel != null) 
                dialogPanel.SetActive(false); // Закрываем диалог
        }
    }

    // Переключает видимость панели
    private void ToggleDialog()
    {
        if (dialogPanel != null)
        {
            bool isActive = !dialogPanel.activeSelf;
            dialogPanel.SetActive(isActive);

            // Опционально: пауза игры при открытии диалога
            Time.timeScale = isActive ? 0f : 1f; 
        }
    }
}