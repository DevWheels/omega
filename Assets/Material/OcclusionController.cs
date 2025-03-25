using UnityEngine;

public class OcclusionController : MonoBehaviour
{
    public Transform player; // ������ �� ���������
    public Camera mainCamera; // ������ �� �������� ������
    public float alphaHidden = 0.5f; // �����-�������� ��� �������� ���������
    public float alphaVisible = 1f; // �����-�������� ��� �������� ���������

    private SpriteRenderer playerRenderer;

    void Start()
    {
        playerRenderer = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // � Raycast ��� ��������, ������������ �� ������ ���������
        RaycastHit2D hit = Physics2D.Raycast(mainCamera.transform.position, player.position - mainCamera.transform.position);
        if (hit.collider != null && hit.collider.gameObject != player.gameObject)
        {
            // ���� ������ ������������ ���������, �� �������� ��� ���������
            Color color = playerRenderer.color;
            color.a = alphaHidden; // ��������� �����-�����
            playerRenderer.color = color;
        }
        else
        {
            // ��������������� ���������
            Color color = playerRenderer.color;
            color.a = alphaVisible; // ������ ���������
            playerRenderer.color = color;
        }
    }
}
