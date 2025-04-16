using UnityEngine;
using UnityEngine.Tilemaps;

// �� �������� �������� ���� using ��� ������ � UI
public class MousePositionHandler : MonoBehaviour
{
    public Tilemap tilemap; // ������ �� ��� �������
    public GameObject prefabToPlace; // ������, ������� ����� ��������������� (��������, ����)
    public GameObject highlightPrefab; // ������ ��� ��������� �����
    private GameObject highlightInstance; // ��������� ���������
    private Vector3Int currentCellPosition; // ������� ������� �����
    public Vector2Int offset; // �������� ��� ��������� ������
    private float lastChangeTime; // ����� ���������� ��������� ������
    public float changeInterval = 0.1f; // �������� ����� ����������� ������
    private bool isButtonActive = false; // ��������� ������
    void Start()
    {
        highlightInstance = Instantiate(highlightPrefab); // ������� ��������� ���������
        highlightInstance.SetActive(false); // �������� ��������� �� ���������
    }
    void Update()
    {
        if (!isButtonActive) return; // ���� ������ �� �������, ������� �� ������
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
        mouseWorldPosition.z = 0;
        Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
        if (tilemap.HasTile(cellPosition))
        {
            highlightInstance.transform.position = tilemap.GetCellCenterWorld(cellPosition);
            highlightInstance.SetActive(true);
            currentCellPosition = cellPosition;
        }
        else
        {
            highlightInstance.SetActive(false);
        }
        // ���������, ������ �� ����� ������ ����
        if (Input.GetMouseButton(0) && highlightInstance.activeSelf)
        {
            // ���������, ������ �� ���������� ������� � ���������� ���������
            if (Time.time - lastChangeTime >= changeInterval)
            {
                ChangeTilesInArea(currentCellPosition, offset);
                lastChangeTime = Time.time; // ��������� ����� ���������� ���������
            }
        }
    }
    public void ToggleButton() // ����� ��� ������������ ��������� ������
    {
        isButtonActive = !isButtonActive; // ����������� ���������
    }
    void ChangeTilesInArea(Vector3Int centerPosition, Vector2Int offset)
    {
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                // ��������� �������� � �������
                Vector3Int position = new Vector3Int(centerPosition.x + x + offset.x, centerPosition.y + y + offset.y, centerPosition.z);
                // ���������, ���� �� �� ������� ����
                Collider2D hit = Physics2D.OverlapPoint(tilemap.GetCellCenterWorld(position));
                if (hit != null && hit.CompareTag("Water"))
                {
                    // ������� ����, ���� �� ����������
                    if (tilemap.HasTile(position))
                    {
                        tilemap.SetTile(position, null); // ������� ����
                    }
                    // ������������� ������ �� �� �� �������
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(position);
                    Instantiate(prefabToPlace, worldPosition, Quaternion.identity); // ������������� ������
                }
            }
        }
    }
}
