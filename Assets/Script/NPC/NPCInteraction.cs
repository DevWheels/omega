using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel; // Основная панель UI
    public TextMeshProUGUI dialogText; // TMP текст
    public AudioSource audioSource; // Источник звука для голоса NPC

    [Header("Dialog Settings")]
    public Dialog[] dialogs; // Список реплик NPC
    private int currentDialogIndex = 0;
    private PlayerMovement playerMovement; // Ссылка на скрипт управления игроком

    private void Start()
    {
        panel.SetActive(false);
        // Находим скрипт управления игроком по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Отключаем управление игроком
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }

            panel.SetActive(true);
            ShowNextDialog();
            Debug.Log("Диалог начат! Управление отключено.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            panel.SetActive(false);
            currentDialogIndex = 0; // Сброс диалога при выходе

            // Включаем управление игроком
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            Debug.Log("Диалог завершен! Управление возвращено.");
        }
    }

    private void Update()
    {
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextDialog();
        }
    }

    private void ShowNextDialog()
    {
        if (currentDialogIndex < dialogs.Length)
        {
            // Установка текста
            dialogText.text = dialogs[currentDialogIndex].text;
            
            // Воспроизведение голоса
            if (dialogs[currentDialogIndex].voiceClip != null)
            {
                audioSource.Stop();
                audioSource.clip = dialogs[currentDialogIndex].voiceClip;
                audioSource.Play();
            }
            
            currentDialogIndex++;
        }
        else
        {
            // Закрываем панель когда диалоги закончились
            panel.SetActive(false);
            currentDialogIndex = 0;

            // Включаем управление игроком при завершении диалога
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }
        }
    }
}

[System.Serializable]
public class Dialog
{
    public string text; // Текст реплики
    public AudioClip voiceClip; // Звуковой файл с голосом
}