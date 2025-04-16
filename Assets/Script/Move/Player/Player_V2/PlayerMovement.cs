using UnityEngine;
using Mirror;
public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;
    public float stairSpeedReductionFactor = 0.25f; // ����������� ������ ���������� �������� �� ��������
    public float ladderClimbSpeed; // �������� ������� �� �������� (����� �����������)
    public Rigidbody2D rb;
    public Animator animator;

    private bool isOnStairs = false; // ����, �����������, ��������� �� ����� �� ��������
    private Vector2 movement; // ��������� ���������� �� ������ ������
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    void Start()
    {
        // ������������� ��������� �������� ������� �� ��������
        ladderClimbSpeed = moveSpeed * stairSpeedReductionFactor; // ��������� �������� �� ������
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        // �������� ������ ��������
        movement = Vector2.zero;

        // ��������� ������� ������ � ������ ����������� ��������
        if (Input.GetKey(KeyCode.W)) // �����
        {
            if (isOnStairs)
            {
                movement.y += 1; // ����������� y ��� �������
            }
            else
            {
                movement.y += 1; // �������� ����� �� �����
            }
        }
        if (Input.GetKey(KeyCode.S)) // ����
        {
            if (isOnStairs)
            {
                movement.y -= 1; // ��������� y ��� ������
            }
            else
            {
                movement.y -= 1; // �������� ���� �� �����
            }
        }
        if (Input.GetKey(KeyCode.A)) // �����
        {
            movement.x -= 1;
        }
        if (Input.GetKey(KeyCode.D)) // ������
        {
            movement.x += 1;
        }

        // ����������� ������ �������� ��� ����������� ��������
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // �������� �� �������� �����������
        movement *= moveSpeed;

        // ������������� ��������� ��������
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        CameraMovement();
    }

    private void CameraMovement()
    {

        mainCam.transform.localPosition = new Vector3(transform.position.x, transform.position.y, -9f);
        transform.position = Vector2.MoveTowards(transform.position,mainCam.transform.localPosition,Time.deltaTime);

    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        // ���������� �������� ��������
        float currentSpeed = isOnStairs ? ladderClimbSpeed : moveSpeed;

        // ������� ������
        Vector2 moveDirection = movement.normalized * currentSpeed * Time.fixedDeltaTime;

        // ������� ������
        rb.MovePosition(rb.position + moveDirection);
        CameraMovement();

    }

    // ����� ��� ��������� �����, ������������, ��������� �� ����� �� ��������
    public void SetOnStairs(bool onStairs)
    {
        isOnStairs = onStairs;
    }
}
