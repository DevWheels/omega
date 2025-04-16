using System.Collections;
using UnityEngine;
using Mirror;


//��� ������ ����� ���������� � ������� ������, ��� �� �������� ��� ������ ���������
//� ��������� scale x = 0 y = 0
public class InventoryManager : NetworkBehaviour
{
    public GameObject inventorySlots;
    public GameObject inventoryQuests;
    public GameObject inventorySkills;
    private bool isOpened = false;
    private PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        playerUI = gameObject.GetComponent<PlayerUI>();
        StartCoroutine(FindInventories());

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (inventorySlots == null || inventorySkills == null || inventoryQuests == null)
        {
            inventorySlots = GameObject.Find("PanelInvent");
            inventorySkills = GameObject.Find("PanelProgres");
            inventoryQuests = GameObject.Find("PanelKvest");

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerUI.UpdateUI();
            if (!isOpened)
            {
                InventoryOpen();
            }
            else
            {
                InventoryClose();
            }
        }

    }
    [Client]
    private void InventoryOpen()
    {

        inventorySlots.transform.localScale = Vector3.one;
        inventoryQuests.transform.localScale = Vector3.one;
        inventorySkills.transform.localScale = Vector3.one;
        isOpened = true;
    }
    [Client]
    private void InventoryClose()
    {
        inventorySlots.transform.localScale = Vector3.zero;
        inventoryQuests.transform.localScale = Vector3.zero;
        inventorySkills.transform.localScale = Vector3.zero;
        isOpened = false;
    }
    private IEnumerator FindInventories()
    {
        while (inventorySlots == null || inventorySkills == null || inventoryQuests == null)
        {
            inventorySlots = GameObject.Find("PanelInvent");
            inventorySkills = GameObject.Find("PanelProgres");
            inventoryQuests = GameObject.Find("PanelKvest");

            yield return null; // ��� ���� ����, ����� �������� ���������

        }
    }

}
