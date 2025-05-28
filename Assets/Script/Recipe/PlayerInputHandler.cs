using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }
    
    public event System.Action OnInteractKeyPressed;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteractKeyPressed?.Invoke();
        }
    }
}