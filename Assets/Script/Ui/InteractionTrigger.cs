using UnityEngine;
using UnityEngine.UI;

public class InteractionTrigger : MonoBehaviour
{
    public GameObject hintUI;      // Подсказка (Нажми E)
    public GameObject interactionUI; // Основной UI
    public float interactionDistance = 3f; // Макс. дистанция взаимодействия

    private Transform player;
    private bool isPlayerNear;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hintUI.SetActive(false);
        interactionUI.SetActive(false);
    }

    private void Update()
    {
        // Проверяем расстояние до игрока
        float distance = Vector3.Distance(transform.position, player.position);
        isPlayerNear = distance <= interactionDistance;

        // Показываем/скрываем подсказку
        hintUI.SetActive(isPlayerNear && !interactionUI.activeSelf);

        // Если игрок рядом и нажал E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            interactionUI.SetActive(!interactionUI.activeSelf);
        }
    }
}