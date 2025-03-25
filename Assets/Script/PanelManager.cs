using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public GameObject[] panels; // ������ ��� �������� �������
    public Button[] buttons; // ������ ��� �������� ������
    private bool isOpened = false;
    private int lastOpened;
    void Start()
    {
        // ����������� ������ � �������
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // ��������� ���������� ��� ����������� ������� � ������-���������
            buttons[i].onClick.AddListener(() => ShowPanel(index));
        }

        // ������������ ��� ������, ����� ������
        //ShowPanel(0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            if (!isOpened)
            {
                panels[lastOpened].transform.localScale = Vector3.one;
                isOpened = true;
            }
            else
            {
                panels[lastOpened].transform.localScale = Vector3.zero;
                isOpened = false;
            }
        }
    }
    // ����� ��� ��������� ������ ������
    public void ShowPanel(int panelIndex)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (i == panelIndex)
            {
                panels[i].transform.localScale = Vector3.one;
                lastOpened = i;
            }
            else
            {
                panels[i].transform.localScale = Vector3.zero;
            }
            //panels[i].SetActive(i == panelIndex); // ���������� ������ ������, ��������� ������������
        }
    }
}