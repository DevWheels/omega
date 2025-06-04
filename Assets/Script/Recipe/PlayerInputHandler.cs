using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler Instance { get; private set; }
    
    public event System.Action OnInteractKeyPressed;
    
    private bool _canInteract = false;
    private Building _nearbyBuilding;
    
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
        if (Input.GetKeyDown(KeyCode.E) && _canInteract)
        {
            OnInteractKeyPressed?.Invoke();
            TryOpenBuildingUI();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _nearbyBuilding = other.GetComponent<Building>();
            if (_nearbyBuilding != null)
            {
                _canInteract = true;
                // Можно показать подсказку "Нажмите E для взаимодействия"
                Debug.Log("Press E to interact with building");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            _canInteract = false;
            _nearbyBuilding = null;
            // Скрыть подсказку
            Debug.Log("Left building interaction zone");
        }
    }
    
    private void TryOpenBuildingUI()
    {
        if (_nearbyBuilding != null)
        {
            _nearbyBuilding.TryOpenBuildingUI();
        }
    }
}