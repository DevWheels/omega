using UnityEngine;
using TMPro;
using UnityEngine.UI; // Добавляем для работы с Image (Filled)

public class TeleportZoneOut : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject timerPanel; // Новая панель с таймером
    public TMP_Text timerText;
    public Image progressBar; // Ссылка на компонент Image с типом Filled
    
    [Header("Teleport Settings")]
    public GameObject targetMarker;
    public bool setGreenZone = false;
    private const float TELEPORT_DELAY = 15f;
    
    private GameObject player; 
    private PlayerSkillController playerSkillController;
    private float remainingTime = 0f;
    private bool isCounting = false;

    private void Update()
    {
        if (isCounting)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0f)
            {
                TeleportPlayer();
                ResetTimer();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();
        }
        
        if (progressBar != null)
        {
            progressBar.fillAmount = remainingTime / TELEPORT_DELAY;
        }
    }

    private void ResetTimer()
    {
        isCounting = false;
        remainingTime = 0f;
        timerPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            player = other.gameObject;
            var ps = player.GetComponent<PlayerSkillController>();
            if (ps.Playermovement.isLocalPlayer) 
            {
                playerSkillController = ps;
                timerPanel.SetActive(true); // Активируем панель таймера
                isCounting = true;
                remainingTime = TELEPORT_DELAY;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            if (other.gameObject == player)
            {
                ResetTimer();
                player = null;
                playerSkillController = null;
            }
        }
    }

    private void TeleportPlayer()
    {
        if (player != null && targetMarker != null)
        {
            player.transform.position = targetMarker.transform.position;
            playerSkillController?.TeleportToGreenZone();
        }
    }

    // Эти методы можно удалить, если они больше не нужны
    public void ConfirmTeleport()
    {
        TeleportPlayer();
        ResetTimer();
    }

    public void CancelTeleport()
    {
        ResetTimer();
    }
}