using UnityEngine;

public class ElementInteraction : MonoBehaviour
{
    public GameObject firePrefab; // ������ ����
    public GameObject icePrefab; // ������ ����
    public GameObject waterPrefab; // ������ ����

    private GameObject currentWater; // ������ �� ������� ������ ����

    void Update()
    {
        CheckElementInteractions();
    }

    private void CheckElementInteractions()
    {
        // �������� ��� ������� � �����
        GameObject[] fireObjects = GameObject.FindGameObjectsWithTag("Fire");
        GameObject[] iceObjects = GameObject.FindGameObjectsWithTag("Ice");

        foreach (GameObject fire in fireObjects)
        {
            foreach (GameObject ice in iceObjects)
            {
                // ��������� ���������� ����� ����� � �����
                if (Vector3.Distance(fire.transform.position, ice.transform.position) <= 0.5f)
                {
                    CreateWaterAtInteractionPoint(fire.transform.position, ice.transform.position);
                }
            }
        }
    }

    private void CreateWaterAtInteractionPoint(Vector3 firePosition, Vector3 icePosition)
    {
        // ���� ���� ��� �������, �������
        if (currentWater != null)
        {
            return;
        }

        // ������� ������� ������� ����� ����� � �����
        Vector3 waterPosition = (firePosition + icePosition) / 2;

        // ������� ������ ����
        currentWater = Instantiate(waterPrefab, waterPosition, Quaternion.identity);

        // ������� ����� � ��� �� �����
        GameObject fireObject = GameObject.FindGameObjectWithTag("Fire");
        GameObject iceObject = GameObject.FindGameObjectWithTag("Ice");

        if (fireObject != null)
        {
            Destroy(fireObject); // ������� ������ ����
        }

        if (iceObject != null)
        {
            Destroy(iceObject); // ������� ������ ����
        }
    }

    public void RemoveWater()
    {
        // ������� ������ ����, ���� �� ����������
        if (currentWater != null)
        {
            Destroy(currentWater);
            currentWater = null;
        }
    }
}
