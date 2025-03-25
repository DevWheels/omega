using System.Collections;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    public Light lightSource; // ������ �� �������� �����
    public float targetIntensity = 0.30f; // ������� �������������
    public float returnIntensity = 0.80f; // �������������, �� ������� ����� ���������
    public float durationToReduce = 10f; // ����� ��� ��������� �������������
    public float delayBeforeReturn = 30f; // ����� �������� ����� ���������

    void Start()
    {
        // ��������� �������� ��� ���������� �������������� �����
        StartCoroutine(AdjustLightIntensity());
    }

    private IEnumerator AdjustLightIntensity()
    {
        // �������� ������������� �����
        float startIntensity = lightSource.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < durationToReduce)
        {
            lightSource.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / durationToReduce);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �� ���������� �����
        }

        // ������������� ������������� �������������
        lightSource.intensity = targetIntensity;

        // ���� 30 ������
        yield return new WaitForSeconds(delayBeforeReturn);

        // ���������� ������������� �����
        elapsedTime = 0f;
        while (elapsedTime < durationToReduce)
        {
            lightSource.intensity = Mathf.Lerp(targetIntensity, returnIntensity, elapsedTime / durationToReduce);
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �� ���������� �����
        }

        // ������������� ������������� �������������
        lightSource.intensity = returnIntensity;
    }
}
