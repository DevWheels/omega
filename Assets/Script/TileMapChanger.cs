using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapChanger : MonoBehaviour
{
    public Tilemap tilemap; // ������ �� ��� �������
    public Tile newTile; // ����� ���� ��� ������

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���������, ������ �� ����� ������ ����
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int Grid = tilemap.WorldToCell(mouseWorldPosition); // ����������� � ���������� �����

            ChangeTilesInArea(Grid);
        }
    }

    void ChangeTilesInArea(Vector3Int center)
    {
        // �������� ������ �������, ���� �����
        int radius = 4;

        for (int x = center.x - radius; x <= center.x + radius; x++)
        {
            for (int y = center.y - radius; y <= center.y + radius; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(position)) // �������� ������� ����� �� �������
                {
                    tilemap.SetTile(position, newTile); // ������ �����
                }
            }
        }
    }
}
