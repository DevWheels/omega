using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������ �������
        if (other.CompareTag("Player"))
        {
            // �������� ��������� PlayerMovement � ������������� ���� isOnStairs � true
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetOnStairs(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // ���������, �������� �� ������ �������
        if (other.CompareTag("Player"))
        {
            // �������� ��������� PlayerMovement � ������������� ���� isOnStairs � false
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetOnStairs(false);
            }
        }
    }
}
