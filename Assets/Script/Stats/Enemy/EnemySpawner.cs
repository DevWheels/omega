using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : NetworkBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxTotalEnemies = 5; // Максимальное общее количество врагов
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float initialSpawnDelay = 3f;
    [SerializeField] private float spawnPointRadius = 2f;
    [SerializeField] private string spawnPointTag = "PointEnemy";

    [Header("Debug")]
    [SerializeField] private bool showSpawnGizmos = true;
    [SerializeField] private bool autoCollectSpawnPoints = true;

    private List<Transform> spawnPoints = new List<Transform>();
    private Dictionary<Transform, bool> pointOccupied = new Dictionary<Transform, bool>(); // Отслеживание занятости точек
    private int currentEnemiesCount = 0;
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
        pointOccupied.Clear();
        
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag(spawnPointTag);
        
        foreach (GameObject point in pointObjects)
        {
            var pointTransform = point.transform;
            spawnPoints.Add(pointTransform);
            pointOccupied[pointTransform] = false;
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
        if (Time.time >= nextSpawnTime && spawnPoints.Count > 0 && currentEnemiesCount < maxTotalEnemies)
        {
            TrySpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    [Server]
    private void TrySpawnEnemy()
    {
        // Создаем список доступных точек (не занятых)
        List<Transform> availablePoints = new List<Transform>();
        foreach (var point in spawnPoints)
        {
            if (!pointOccupied[point])
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0) return;

        // Выбираем случайную доступную точку
        Transform selectedPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        Vector2 spawnPosition = GetSpawnPosition(selectedPoint);
        
        if (spawnPosition != Vector2.zero)
        {
            SpawnEnemy(spawnPosition, selectedPoint);
        }
    }

    [Server]
    private Vector2 GetSpawnPosition(Transform spawnPoint)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 spawnPos = (Vector2)spawnPoint.position + 
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
    private void SpawnEnemy(Vector2 position, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        NetworkServer.Spawn(enemy);
        
        currentEnemiesCount++;
        pointOccupied[spawnPoint] = true;
        
        enemy.GetComponent<TestenemyHealth>().OnDeath += () => OnEnemyDeath(spawnPoint);
        
        Debug.Log($"Spawned enemy at {position}. Total enemies: {currentEnemiesCount}/{maxTotalEnemies}");
    }

    [Server]
    private void OnEnemyDeath(Transform spawnPoint)
    {
        currentEnemiesCount--;
        if (currentEnemiesCount < 0) currentEnemiesCount = 0;
        
        pointOccupied[spawnPoint] = false;
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