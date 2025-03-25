using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public XpBar targetedEnemy; // ������� ���� ��� �����
    public int skillDamage = 100; // ���� ������
    public Button attackButton; // ������ ��� �����
    public TMP_Text targetText; // ������ �� ��������� ���� ��� ����������� ����

    private void Start()
    {
        attackButton.onClick.AddListener(Attack);
        UpdateTargetText();
    }

    // ����� ��� ������ �����, ��� ����� �� ����
    private void OnMouseDown()
    {
        // �������� ������� ������� � ������� �����������
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���������, ���� �� ��������� � ���� �����
        Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);
        if (hitCollider != null)
        {
            // ���������, �������� �� ��������� ������
            XpBar enemy = hitCollider.GetComponent<XpBar>();
            if (enemy != null)
            {
                SelectTarget(enemy);
            }
        }
    }

    // ����� ��� ������ ����
    public void SelectTarget(XpBar enemy)
    {
        targetedEnemy = enemy;
        UpdateTargetText();
    }

    // ����� ��� �����
    private void Attack()
    {
        if (targetedEnemy != null)
        {
            targetedEnemy.TakeDamage(skillDamage);
        }
        else
        {
            Debug.Log("No target selected!");
        }
    }

    // ��������� ����� � ����������� � ����
    private void UpdateTargetText()
    {
        if (targetedEnemy != null)
        {
            targetText.text = $"{targetedEnemy.name}"; // ���������� ��� ���������� �����
        }
        else
        {
            targetText.text = "Target: None";
        }
    }

}
