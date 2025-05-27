using UnityEngine;
using UnityEngine.UI;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI; // Ссылка на UI элемент
    [SerializeField] private float interactionDistance = 2f; // Дистанция взаимодействия
    
    private Transform playerTransform;
    private bool isPlayerNearby;

    private void Start()
    {
        // Находим игрока по тегу
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Скрываем UI при старте
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (playerTransform == null) return;
        
        // Проверяем дистанцию до игрока
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distance <= interactionDistance)
        {
            if (!isPlayerNearby)
            {
                isPlayerNearby = true;
                ShowUI();
            }
        }
        else
        {
            if (isPlayerNearby)
            {
                isPlayerNearby = false;
                HideUI();
            }
        }
    }

    private void ShowUI()
    {
        if (interactionUI != null)
            interactionUI.SetActive(true);
    }

    private void HideUI()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    // Рисуем gizmo для визуализации радиуса взаимодействия
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}