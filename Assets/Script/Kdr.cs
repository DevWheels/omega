using UnityEngine;
using UnityEngine.UI;

public class Kdr : MonoBehaviour
{
    public Button skillButton; // ������ �� ������ ������
    public Image cooldownOverlayImage; // ������ �� ����������� � ���������� �������� ����������

    private bool _isCooldown = false; // ���� ����������� ������
    private float lastActivationTime; // ����� ���������� �������������

    private void Start()
    {
        // ���������� ������ � ������ ������
        skillButton.onClick.AddListener(ActivateSkill);
    }

    private void Update()
    {
        // ���� ����� ��������� �� ��������, �� ������ ������������
        if (_isCooldown)
        {
            float cooldown = 10f; // ����� �� � ��������
            float timeLeft = cooldown - (Time.time - lastActivationTime);

            if (timeLeft <= 0)
            {
                _isCooldown = false;
                cooldownOverlayImage.fillAmount = 0f;
                cooldownOverlayImage.gameObject.SetActive(false);
                skillButton.interactable = true; // �������� ������ �����
                return;
            }

            cooldownOverlayImage.fillAmount = timeLeft / cooldown;
        }
    }

    private void ActivateSkill()
    {
        if (_isCooldown) return; // ���� ����� �� �� - �� �� �� ����� ��� ������������
        _isCooldown = true; // ����� � ��
        lastActivationTime = Time.time; // ���������� ����� ���������� �������������
        cooldownOverlayImage.gameObject.SetActive(true); // ���������� ���������� ������ ��

        skillButton.interactable = false; // ��������� ������ ������
    }
}
