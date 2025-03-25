using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator animator; // ������ �� ��������� Animator
    public Transform player; // ������ �� ���������

    private Vector3 lastPosition; // ������ ��������� ������� ���������

    void Start()
    {
        lastPosition = player.position; // �������������� ��������� �������
    }

    void Update()
    {
        // �������� ������� ���� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ������� Z-���������

        // ��������� ����������� �� ��������� � ����
        Vector3 direction = mousePosition - player.position;

        // ���������, �������� �� ��������
        if (player.position != lastPosition)
        {
            // ���� �������� ��������, ��������� ��������� �������
            lastPosition = player.position;
            return; // ������� �� ������, ����� �� ��������� ��������
        }

        // ��������� �������� � ����������� �� ��������� ����
        UpdateAnimation(direction);
    }

    private void UpdateAnimation(Vector3 direction)
    {
        // ���������� ��������, ����� �������� ����������
        animator.ResetTrigger("lookUp");
        animator.ResetTrigger("lookDown");
        animator.ResetTrigger("lookRight");
        animator.ResetTrigger("lookLeft");

        // ���������� ���������� �������� ��� ����������� �����������
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // �������������� ����������� �����������
            if (direction.x > 0)
            {
                // ���� ������ �� ���������
                animator.SetTrigger("lookLeft");
            }
            else
            {
                // ���� ����� �� ���������
                animator.SetTrigger("lookRight");
            }
        }
        else
        {
            // ������������ ����������� �����������
            if (direction.y > 0)
            {
                // ���� ���� ���������
                animator.SetTrigger("lookUp");
            }
            else
            {
                // ���� ���� ���������
                animator.SetTrigger("lookDown");
            }
        }
    }
}