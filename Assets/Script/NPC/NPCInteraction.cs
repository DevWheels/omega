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
    private bool isDialogActive = false; // Флаг активности диалога
    private Collider2D npcCollider; // Коллайдер NPC

    private void Start()
    {
        panel.SetActive(false);
        npcCollider = GetComponent<Collider2D>();
        
        // Находим скрипт управления игроком по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDialogActive)
        {
            StartDialog();
        }
    }

    private void StartDialog()
    {
        isDialogActive = true;
        
        // Отключаем управление игроком
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Отключаем коллайдер, чтобы игрок не мог выйти из триггера
        if (npcCollider != null)
        {
            npcCollider.enabled = false;
        }

        panel.SetActive(true);
        ShowNextDialog();
        Debug.Log("Диалог начат! Управление отключено.");
    }

    private void Update()
    {
        if (isDialogActive && Input.GetKeyDown(KeyCode.E))
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
            EndDialog();
        }
    }

    private void EndDialog()
    {
        // Закрываем панель когда диалоги закончились
        panel.SetActive(false);
        currentDialogIndex = 0;
        isDialogActive = false;

        // Включаем управление игроком
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Включаем коллайдер обратно
        if (npcCollider != null)
        {
            npcCollider.enabled = true;
        }

        Debug.Log("Диалог завершен! Управление возвращено.");
    }
}

[System.Serializable]
public class Dialog
{
    public string text; // Текст реплики
    public AudioClip voiceClip; // Звуковой файл с голосом
}