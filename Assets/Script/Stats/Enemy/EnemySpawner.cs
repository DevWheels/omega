using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : NetworkBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float initialSpawnDelay = 3f;
    [SerializeField] private float spawnPointRadius = 2f;
    [SerializeField] private string spawnPointTag = "PointEnemy";

    [Header("Debug")]
    [SerializeField] private bool showSpawnGizmos = true;
    [SerializeField] private bool autoCollectSpawnPoints = true;

    private List<Transform> spawnPoints = new List<Transform>();
    private float nextSpawnTime;

    public override void OnStartServer()
    {
        base.OnStartServer();
        
        if (autoCollectSpawnPoints)
        {
            CollectSpawnPoints();
        }
        
        nextSpawnTime = Time.time + initialSpawnDelay;
    }

    [Server]
    private void CollectSpawnPoints()
    {
        spawnPoints.Clear();
        
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag(spawnPointTag);
        
        foreach (GameObject point in pointObjects)
        {
            spawnPoints.Add(point.transform);
            Debug.Log($"Added spawn point: {point.name}");
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points found with tag: " + spawnPointTag);
        }
    }

    [ServerCallback]
    private void Update()
    {
        if (Time.time >= nextSpawnTime && spawnPoints.Count > 0)
        {
            TrySpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    [Server]
    private void TrySpawnEnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemies)
            return;

        Vector2 spawnPosition = GetSpawnPosition();
        if (spawnPosition != Vector2.zero)
        {
            SpawnEnemy(spawnPosition);
        }
    }

    [Server]
    private Vector2 GetSpawnPosition()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        for (int i = 0; i < 5; i++)
        {
            Vector2 spawnPos = (Vector2)randomSpawnPoint.position + 
                             Random.insideUnitCircle * spawnPointRadius;
            
            if (IsValidSpawnPosition(spawnPos))
                return spawnPos;
        }

        return Vector2.zero;
    }

    [Server]
    private bool IsValidSpawnPosition(Vector2 position)
    {

        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (Vector2.Distance(position, player.transform.position) < 3f)
                return false;
        }


        Collider2D hit = Physics2D.OverlapCircle(position, 0.5f);
        return hit == null;
    }

    [Server]
    private void SpawnEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(enemy);
        Debug.Log($"Spawned enemy at {position} near {spawnPointTag} point");
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnGizmos) return;


        Gizmos.color = Color.magenta;
        foreach (var point in spawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.3f);
                Gizmos.DrawWireSphere(point.position, spawnPointRadius);
            }
        }
    }
}