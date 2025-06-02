using UnityEngine;
public class PlayerSmithyUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject PanelSmithy;
    [SerializeField] private GameObject PanelSmithyBuilding;

    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool isPlayerNearSmithy = false;

    private void Awake()
    {
        // Поиск даже скрытых объектов
        if (PanelSmithy == null)
            PanelSmithy = FindObjectInScene("PanelSmithy");
        
        if (PanelSmithyBuilding == null)
            PanelSmithyBuilding = FindObjectInScene("PanelSmithyBuilding");
    }
    private GameObject FindObjectInScene(string name)
    {
        // Ищем все объекты в сцене, включая неактивные
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var obj in allObjects)
        {
            if (obj.name == name && obj.scene.isLoaded)
            {
                return obj;
            }
        }
        Debug.LogError($"Не удалось найти объект {name} в сцене!");
        return null;
    }
    private void Start()
    {
        if (PanelSmithy != null) PanelSmithy.SetActive(true);
        if (PanelSmithyBuilding != null) PanelSmithyBuilding.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && isPlayerNearSmithy && PanelSmithyBuilding != null)
        {
            PanelSmithyBuilding.SetActive(!PanelSmithyBuilding.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SmithyBuilding"))
        {
            isPlayerNearSmithy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SmithyBuilding"))
        {
            isPlayerNearSmithy = false;
            if (PanelSmithyBuilding != null)
                PanelSmithyBuilding.SetActive(false);
        }
    }
}
