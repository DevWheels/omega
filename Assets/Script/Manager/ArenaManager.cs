using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class ArenaManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public float spawnRadius = 1f;
        [HideInInspector] public float nextSpawnTime;
    }

    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject prefab;
        public float spawnWeight = 1f;
        public int maxInstances = 5;
        [HideInInspector] public int currentInstances;
    }

    [Header("Spawn Settings")]
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public List<SpawnableItem> spawnableItems = new List<SpawnableItem>();
    public float initialSpawnDelay = 5f;
    public float respawnInterval = 60f;
    public int maxTotalItems = 20;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private bool isInitialized = false;

    private void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        isInitialized = true;
        Invoke(nameof(SpawnInitialItems), initialSpawnDelay);
        InvokeRepeating(nameof(RespawnItems), initialSpawnDelay + respawnInterval, respawnInterval);
    }

    private void SpawnInitialItems()
    {
        foreach (var point in spawnPoints)
        {
            TrySpawnItemAtPoint(point);
            point.nextSpawnTime = Time.time + respawnInterval;
        }
    }

    private void RespawnItems()
    {
        spawnedItems.RemoveAll(item => item == null);

        foreach (var point in spawnPoints)
        {
            if (Time.time >= point.nextSpawnTime && spawnedItems.Count < maxTotalItems)
            {
                TrySpawnItemAtPoint(point);
                point.nextSpawnTime = Time.time + respawnInterval;
            }
        }
    }

    private void TrySpawnItemAtPoint(SpawnPoint spawnPoint)
    {
        if (spawnableItems.Count == 0) return;

        SpawnableItem itemToSpawn = GetRandomItemToSpawn();
        if (itemToSpawn == null) return;

        Vector3 spawnPosition = spawnPoint.point.position + 
                              Random.insideUnitSphere * spawnPoint.spawnRadius;
        spawnPosition.y = spawnPoint.point.position.y;

        GameObject spawnedItem = Instantiate(itemToSpawn.prefab, spawnPosition, 
                                           Quaternion.identity);
        spawnedItems.Add(spawnedItem);
        itemToSpawn.currentInstances++;

 
        var pickup = spawnedItem.GetComponent<PickUpObject>() ?? spawnedItem.AddComponent<PickUpObject>();
        

        if (spawnedItem.GetComponent<ItemBase>() == null)
        {
            Debug.LogWarning($"Prefab {spawnedItem.name} doesn't have ItemBase component!");
        }
    }

    private SpawnableItem GetRandomItemToSpawn()
    {
        var availableItems = spawnableItems.FindAll(item => 
            item.currentInstances < item.maxInstances);

        if (availableItems.Count == 0) return null;

        float totalWeight = 0;
        foreach (var item in availableItems)
        {
            totalWeight += item.spawnWeight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var item in availableItems)
        {
            currentWeight += item.spawnWeight;
            if (randomValue <= currentWeight)
            {
                return item;
            }
        }

        return availableItems[0];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (var point in spawnPoints)
        {
            if (point.point != null)
            {
                Gizmos.DrawWireSphere(point.point.position, point.spawnRadius);
            }
        }
    }
}