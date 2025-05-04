using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float movementThreshold = 0.1f; // Порог для определения движения

    private Vector3 lastPosition;
    private bool isMoving = false;

    void Start()
    {
        lastPosition = player.position;
    }

    void Update()
    {
        // Определяем, движется ли персонаж
        isMoving = Vector3.Distance(player.position, lastPosition) > movementThreshold;
        lastPosition = player.position;

        // Получаем позицию мыши
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Вычисляем направление
        Vector3 direction = mousePosition - player.position;
        direction.Normalize();

        // Обновляем анимацию
        UpdateAnimation(direction, isMoving);
    }

    private void UpdateAnimation(Vector3 direction, bool moving)
    {
        // Устанавливаем параметр движения
        animator.SetBool("isMoving", moving);

        if (!moving)
        {
            // Для 8 направлений в покое
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Нормализуем угол от 0 до 360
            if (angle < 0) angle += 360;

            // Устанавливаем направление взгляда
            animator.SetFloat("directionAngle", angle);
        }
        else
        {
            // Для 8 направлений в движении
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // Нормализуем угол от 0 до 360
            if (angle < 0) angle += 360;

            // Устанавливаем направление движения
            animator.SetFloat("moveAngle", angle);
        }
    }
}