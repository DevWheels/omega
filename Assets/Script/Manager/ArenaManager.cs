using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(NetworkIdentity))]
public class ArenaManager : NetworkBehaviour
{
    public static ArenaManager Instance { get; private set; }

    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] [Range(1f, 60f)] private float spawnInterval = 30f;
    [SerializeField] [Min(1)] private int maxItems = 20;

    [Header("Debug")]
    [SerializeField] private List<GameObject> spawnedItems = new List<GameObject>();
    [SerializeField] private bool debugLogs = true;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (debugLogs) Debug.Log("ArenaManager: Server initialization");
        
        ValidateReferences();
        StartCoroutine(SpawnItemsRoutine());
    }

    private void ValidateReferences()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
            Debug.LogError("ArenaManager: No item prefabs assigned!");
        
        if (spawnPoints == null || spawnPoints.Length == 0)
            Debug.LogError("ArenaManager: No spawn points assigned!");
    }

    private IEnumerator SpawnItemsRoutine()
    {
        if (debugLogs) Debug.Log("ArenaManager: Spawn routine started");

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            CleanupDestroyedItems();

            if (CanSpawnMoreItems())
            {
                SpawnRandomItem();
            }
        }
    }

    private void CleanupDestroyedItems()
    {
        spawnedItems.RemoveAll(item => item == null);
    }

    private bool CanSpawnMoreItems()
    {
        if (spawnedItems.Count >= maxItems)
        {
            if (debugLogs) Debug.Log("ArenaManager: Max items reached, skipping spawn");
            return false;
        }
        return true;
    }

    private void SpawnRandomItem()
    {
        var itemToSpawn = GetRandomPrefab();
        var spawnPoint = GetRandomSpawnPoint();

        if (itemToSpawn == null || spawnPoint == null)
        {
            if (debugLogs) Debug.LogWarning("ArenaManager: Invalid spawn parameters");
            return;
        }

        var spawnedItem = Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(spawnedItem);
        spawnedItems.Add(spawnedItem);

        if (debugLogs) Debug.Log($"ArenaManager: Spawned {itemToSpawn.name} at {spawnPoint.position}");
    }

    private GameObject GetRandomPrefab()
    {
        if (itemPrefabs.Length == 0) return null;
        return itemPrefabs[Random.Range(0, itemPrefabs.Length)];
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints.Length == 0) return null;
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    [Server]
    public void ItemPickedUp(GameObject item)
    {
        if (spawnedItems.Contains(item))
        {
            spawnedItems.Remove(item);
            if (debugLogs) Debug.Log($"ArenaManager: Item {item.name} picked up");
        }
    }

    [Server]
    public void ClearAllItems()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                NetworkServer.Destroy(item);
            }
        }
        spawnedItems.Clear();
        
        if (debugLogs) Debug.Log("ArenaManager: All items cleared");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}