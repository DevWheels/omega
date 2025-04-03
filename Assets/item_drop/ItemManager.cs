using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private List<Iteme> playerInventory = new List<Iteme>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToInventory(Iteme item)
    {
        playerInventory.Add(item);
        Debug.Log($"Added {item.Name1} to inventory");
    }

    public List<Iteme> GetInventory()
    {
        return new List<Iteme>(playerInventory);
    }
}
