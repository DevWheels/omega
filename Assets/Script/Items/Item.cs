using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item1 : ScriptableObject
{
    [Header("������� ��������������")]
    public string Name = " ";
    public string Description = "�������� ��������";
    public Sprite icon = null;

    public bool isHealing;
    public float HealingPower;

    public bool isMana;
    public float ManaPower;

    [Header("������� ��������������")]
    public int time;

    [Header("�������������� ��� ��������")]
    public int Coins;

    public string PlayerPrefsName;
    
}