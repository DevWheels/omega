using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    public GameObject targetUI; // UI, который нужно открыть
    private bool isPlayerInTrigger = false;
    public GameObject hintText; // Текст-подсказка

    private void Update()
    {
        // Если игрок в триггере и нажал E
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            targetUI.SetActive(!targetUI.activeSelf); // Переключаем UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log("Игрок вошел в триггер!"); // Проверка в консоли
            isPlayerInTrigger = true;
            hintText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            Debug.Log("Кнопка E нажата!"); // Проверка в консоли
            targetUI.SetActive(!targetUI.activeSelf);
            isPlayerInTrigger = false;
            hintText.SetActive(false);
            targetUI.SetActive(false);
        }
    }
}