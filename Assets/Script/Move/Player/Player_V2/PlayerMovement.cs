using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    
    private Vector2 movementInput;
    private Vector2 lastDirection;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        lastDirection = Vector2.down; // Начальное направление - вниз
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleInput();
        UpdateAnimator();
        UpdateCamera();
    }

    private void HandleInput()
    {
        movementInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Сохраняем последнее направление при движении
        if (movementInput.magnitude > 0.1f)
        {
            lastDirection = movementInput;
        }
    }

    private void UpdateAnimator()
    {
        // Устанавливаем параметры аниматора
        animator.SetFloat("Horizontal", lastDirection.x);
        animator.SetFloat("Vertical", lastDirection.y);
        animator.SetFloat("Speed", movementInput.magnitude); // Величина движения (0-1)
    }

    private void UpdateCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                mainCamera.transform.position.z
            );
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        rb.linearVelocity = movementInput * moveSpeed;
    }
}