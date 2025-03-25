using System.Collections;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    public Light lightSource; // ������ �� ��������� Light
    public GameObject fireObject; // ������ �� ������ ����
    public float dayDuration = 180f; // ������������ ��� � ��������
    public float nightDuration = 80f; // ������������ ���� � ��������

    private void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>(); // �������� ��������� Light, ���� �� �� ��������
        }

        StartCoroutine(DayNightCycle());
    }

    private IEnumerator DayNightCycle()
    {
        while (true) // ����������� ����
        {
            // ������� ����
            lightSource.enabled = false; // ��������� ����
            if (fireObject != null)
            {
                fireObject.SetActive(false); // ������������ ������ ����
            }

            yield return new WaitForSeconds(dayDuration); // ���� ������������ ���

            // ������ ����
            lightSource.enabled = true; // �������� ���� (���� �����)
            if (fireObject != null)
            {
                fireObject.SetActive(true); // ���������� ������ ����
            }

            yield return new WaitForSeconds(nightDuration); // ���� ������������ ����
        }
    }
}
