using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
    public static LocalPlayer Instance { get; private set; }
    public PlayerInventory Inventory { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        Inventory = GetComponent<PlayerInventory>();
    }
}