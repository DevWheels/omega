using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FierSkil1 : MonoBehaviour
{
    public Tilemap tilemap; // ������ �� ��� �������
    public GameObject prefabToPlace; // ������, ������� ����� ��������������� (��������, �����)
    public GameObject highlightPrefab; // ������ ��� ��������� �����
    private GameObject highlightInstance; // ��������� ���������
    private Vector3Int currentCellPosition; // ������� ������� �����
    public Vector2Int offset; // �������� ��� ��������� ������
    private float lastChangeTime; // ����� ���������� ��������� ������
    public float changeInterval = 0.1f; // �������� ����� ����������� ������
    public float prefabDuration = 5f; // �����, ����� ������� ������ ��������
    private bool isButtonActive = false; // ��������� ������
    // ��������� ���� ��� ��������� �������� z
    public float mouseWorldZPosition = 1.0f; // �������� z ��� ���������������� ����
    public TileBase stoneTile; // ���� ����
    public TileBase fireTile; // ���� ����
    public TileBase waterTile; // ���� ����
    // ������� ��� �������� ������������ ������
    private Dictionary<Vector3Int, TileBase> originalTiles = new Dictionary<Vector3Int, TileBase>();
    void Start()
    {
        highlightInstance = Instantiate(highlightPrefab); // ������� ��������� ���������
        highlightInstance.SetActive(false); // �������� ��������� �� ���������
    }

    void Update()
    {
        // ������������ ��������� ������ � ������� �������, ��������, "Space"
        if (Input.GetKeyDown(KeyCode.Alpha2)) // �������� �� ������ �������
        {
            ToggleButton(); // ����������� ���������
        }
        // ���� ������ �� �������, �������� ���������
        if (!isButtonActive)
        {
            highlightInstance.SetActive(false);
            return; // ������� �� ������
        }
        if (!isButtonActive) return; // ���� ������ �� �������, ������� �� ������
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
        mouseWorldPosition.z = mouseWorldZPosition; // ���������� �������� �� ����������
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
                ChangeSoilTilesInArea(currentCellPosition, offset);
                lastChangeTime = Time.time; // ��������� ����� ���������� ���������
            }
        }
    }
    public void ToggleButton() // ����� ��� ������������ ��������� ������
    {
        isButtonActive = !isButtonActive; // ����������� ��������� 
        highlightInstance.SetActive(isButtonActive); // ������������� ��������� ��������� � ����������� �� ������
    }
    void ChangeSoilTilesInArea(Vector3Int centerPosition, Vector2Int offset)
    {
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                // ��������� �������� � �������
                Vector3Int position = new Vector3Int(centerPosition.x + x + offset.x, centerPosition.y + y + offset.y, centerPosition.z);
                // ���������, ���� �� �� ������� ����
                if (tilemap.HasTile(position) && tilemap.GetTile(position) is TileBase)
                {
                    TileBase originalTile = tilemap.GetTile(position);

                    // ������� ��� ���������� ���� � ����
                    if (originalTile != fireTile && originalTile != waterTile && originalTile != stoneTile)
                    {
                        originalTiles[position] = originalTile; // ��������� ������������ ���� � �������
                        Debug.Log($"Original tile saved at {position}: {originalTile}");
                    }

                 
                }

                // ������������� ����� ����
                tilemap.SetTile(position, fireTile); // ������������� ����� ����, ��������� � ����������

                // �������� �� �������� ����� ��� �������� ����
                CheckForWaterCreation(position);
            }
        }
    }
    public void CheckForWaterCreation(Vector3Int position)
    {
        bool hasIce = false;
        bool hasFire = false;

        // ��������� �������� �������
        Vector3Int[] neighborPositions = {
            new Vector3Int(position.x - 1, position.y, position.z), // �����
            new Vector3Int(position.x + 1, position.y, position.z), // ������
            new Vector3Int(position.x, position.y - 1, position.z), // �����
            new Vector3Int(position.x, position.y + 1, position.z)  // ������
        };

        foreach (var neighbor in neighborPositions)
        {
            if (tilemap.HasTile(neighbor))
            {
                TileBase tile = tilemap.GetTile(neighbor);
                if (tile == waterTile)
                {
                    hasIce = true; // ������ ���� ����
                }
                else if (tile == fireTile)
                {
                    hasFire = true; // ������ ���� ����
                }
            }
        }

        // ���� ��� ����� �������, ������������� ���� ����
        if (hasIce && hasFire)
        {
            tilemap.SetTile(position, stoneTile); // ������������� ���� ����
        }
        else
        {
            // ��������� �������� ��� ������������ �����
            StartCoroutine(RestoreTileAfterDelay(position, 3f)); // �������� 3 ������� ����� ���������
        }
    }
    private IEnumerator RestoreTileAfterDelay(Vector3Int position, float delay)
    {
        yield return new WaitForSeconds(delay); // ���� ��������� �����

        // ���������, �������� �� ���� �����
        if (originalTiles.TryGetValue(position, out TileBase originalTile))
        {
            tilemap.SetTile(position, originalTile); // ��������������� ������������ ���� (����)
            originalTiles.Remove(position); // ������� ������ �� �������
        }
    }
}