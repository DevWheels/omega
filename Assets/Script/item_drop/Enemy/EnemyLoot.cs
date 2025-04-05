using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    public EnemyLootTable lootTable;
    public EnemyRank enemyRank = EnemyRank.Common;
    [Range(0f, 1f)] public float dropChance = 0.7f;
    public int mobLevel = 1;

    public void DropItem(int playerLevel)
    {

        if (Random.value > dropChance || lootTable == null)
        {
            Debug.Log("������� �� ����� (���� ��� ����������� lootTable)");
            return;
        }


        EquipmentItem item = lootTable.GetRandomItem(enemyRank, playerLevel, mobLevel);

        if (item != null)
        {
            SpawnItem(item);
        }
        else
        {
            Debug.LogWarning("�� ������� ������� ������� (��������, ��� ���������� ��������� � lootTable)");
        }
    }

    private void SpawnItem(EquipmentItem item)
    {

        if (item.worldPrefab != null)
        {
            Instantiate(item.worldPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"� �������� {item.itemName} ����������� worldPrefab");
        }


        item.LogStats();
    }
}