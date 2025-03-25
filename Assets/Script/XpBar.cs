using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XpBar : MonoBehaviour
{
    public int health = 100; // ������� �������� �����
    public int armor = 100; // ����� �����

    // ������ �� UI �������� TextMeshPro
    public TMP_Text healthText; // ������ �� TextMeshPro ��� ��������
    public TMP_Text armorText; // ������ �� TextMeshPro ��� �����

    private void Start()
    {
        // ������������� ��������� �������� ������
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        // ���������, ������� ����� �������� ����� (20% �� ��������� �����)
        int damageToArmor = Mathf.FloorToInt(damage * 0.2f);
        int damageToHealth = damage - damageToArmor;

        // ��������� ����� � ��������
        armor -= damageToArmor;
        health -= damageToHealth;

        // ��������� UI
        UpdateUI();

        // ������� ���������� � damage
        Debug.Log($"Damage dealt: {damage}, Damage to armor: {damageToArmor}, Damage to health: {damageToHealth}");
        Debug.Log($"Remaining Armor: {armor}, Remaining Health: {health}");

        // ���������, ���� ���� ����
        if (health <= 0)
        {
            Die();
        }
    }


    private void UpdateUI()
    {
        // ��������� �������� ��������� �����
        healthText.text = $"{health}";
        armorText.text = $"{armor}";
    }

    public void Die()
    {
        // ������� ��������� � ������
        Debug.Log("Enemy has died!");

        // ������������ �� ���������� �����
        // '1' - ��� ���������� ���� �����, �� ������� ����� ���������
        // ��������, ���� � ��� ���� ��������� �����, � �� ������ ��������� �� ����������
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
