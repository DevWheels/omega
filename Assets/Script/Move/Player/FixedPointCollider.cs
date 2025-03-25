using UnityEngine;

public class FixedPointCollider : MonoBehaviour
{
    public Transform player; // ������ �� ���������
    public float radius = 1.5f; // ������, �� ������� ����� ���������� ���������
    public float rotationSpeed = 5f; // �������� ��������

    private Vector3 fixedPoint; // ������������� ����� (������� ������)

    void Start()
    {
        // ������������� ������������� ����� �� ������� ������
        fixedPoint = player.position;
    }

    void Update()
    {
        // ��������� ������������� ����� �� ������� ������
        fixedPoint = player.position;

        // �������� ������� ���� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ������� Z-���������

        // ��������� ����������� �� ������������� ����� � ����
        Vector3 direction = (mousePosition - fixedPoint).normalized;

        // ������� ���� ����� ������������ � �������� ������ ���������
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������������ ��������� � ����
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ���������� ��������� �� ������������� ���������� �� ������������� �����
        transform.position = fixedPoint + direction * radius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��������� ������������ � ������� ���������
        if (other.CompareTag("Water"))
        {
            Debug.Log("Collided with Water!");
        }
    }
}