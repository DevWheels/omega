using UnityEngine;
using TMPro; // ��� ������ � TextMeshPro
using UnityEngine.EventSystems;
using System.Collections; // ��� ������ � ���������

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText; // ������ �� ����� ������
    private Color originalColor; // �������� ���� ������
    private Color targetColor; // ����, � �������� �� ���������
    private float transitionDuration = 0.5f; // ������������ �������� � ��������

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        originalColor = new Color(0.5f, 0.5f, 0.5f); // ����� ����
        buttonText.color = originalColor; // ������������� �������� ���� ������
        targetColor = originalColor; // ���������� ������� ���� ����� ��
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetColor = Color.white; // ���� ������ ��� ���������
        StopAllCoroutines(); // ������������� ��� �������� ����� ������� �����
        StartCoroutine(ChangeColor(originalColor, targetColor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetColor = originalColor; // ������������ � ��������� �����
        StopAllCoroutines(); // ������������� ��� �������� ����� ������� �����
        StartCoroutine(ChangeColor(buttonText.color, targetColor));
    }

    private IEnumerator ChangeColor(Color startColor, Color endColor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            buttonText.color = Color.Lerp(startColor, endColor, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime; // ����������� �����
            yield return null; // ���� ���������� �����
        }

        buttonText.color = endColor; // ������������� �������� ����
    }
}
