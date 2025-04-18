using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
public class ArenaManager : NetworkBehaviour
{
    public static ArenaManager Instance { get; private set; }

    [Header("Spawn Settings")]
    public GameObject[] itemPrefabs; 
    public Transform[] spawnPoints; 
    public float spawnInterval = 30f;
    public int maxItems = 20; 

    [Header("Debug")]
    [SerializeField] private List<GameObject> spawnedItems = new List<GameObject>(); 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(SpawnItemsRoutine());
    }

    private IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);


            spawnedItems.RemoveAll(item => item == null);


            if (spawnedItems.Count >= maxItems)
            {
                continue;
            }


            GameObject itemToSpawn = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


            GameObject spawnedItem = Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);
            

            NetworkServer.Spawn(spawnedItem);
            

            spawnedItems.Add(spawnedItem);
        }
    }


    [Server]
    public void ItemPickedUp(GameObject item)
    {
        if (spawnedItems.Contains(item))
        {
            spawnedItems.Remove(item);
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
    }
}