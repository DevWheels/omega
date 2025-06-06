using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel; // Панель диалога
    public TextMeshProUGUI dialogText; // Текст диалога (TMP)
    public AudioSource audioSource; // Аудиоисточник для голоса NPC

    [Header("Dialog Settings")]
    public Dialog[] dialogs; // Массив реплик NPC
    private int currentDialogIndex = 0;
    private bool isDialogActive = false;
    private Collider2D npcCollider;

    private void Start()
    {
        panel.SetActive(false);
        npcCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDialogActive && IsLocalPlayer(other))
        {
            StartDialog();
        }
    }

    // Проверка, является ли игрок локальным (для Mirror)
    private bool IsLocalPlayer(Collider2D playerCollider)
    {
        NetworkIdentity networkIdentity = playerCollider.GetComponent<NetworkIdentity>();
        return networkIdentity != null && networkIdentity.isLocalPlayer;
    }

    private void StartDialog()
    {
        isDialogActive = true;
        npcCollider.enabled = false; // Отключаем коллайдер NPC, чтобы диалог не прерывался
        panel.SetActive(true);
        ShowNextDialog();
        Debug.Log("Диалог начат (управление не отключено)");
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
            dialogText.text = dialogs[currentDialogIndex].text;
            
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
        panel.SetActive(false);
        currentDialogIndex = 0;
        isDialogActive = false;
        npcCollider.enabled = true; // Включаем коллайдер обратно

        Debug.Log("Диалог завершен (управление не блокировалось)");
    }
}

[System.Serializable]
public class Dialog
{
    public string text; // Текст реплики
    public AudioClip voiceClip; // Озвучка реплики
}