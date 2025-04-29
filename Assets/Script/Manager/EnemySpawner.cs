// using UnityEngine;
//
// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
//
// public class EnemySpawner : MonoBehaviour
// {
//     [System.Serializable]
//     public class EnemyWave
//     {
//         public GameObject enemyPrefab;
//         public int count;
//         public float spawnInterval = 1f;
//         public float delayBeforeWave = 0f;
//         [Range(0f, 1f)] public float eliteChance = 0.1f;
//     }
//
//     [Header("Spawn Settings")]
//     public List<EnemyWave> waves;
//     public float timeBetweenWaves = 5f;
//     public Transform[] spawnPoints;
//     public bool spawnInPlayerRadius = true;
//     public float spawnRadius = 10f;
//
//     [Header("Difficulty Scaling")]
//     public float healthMultiplierPerWave = 1.1f;
//     public float damageMultiplierPerWave = 1.05f;
//     public int maxActiveEnemies = 20;
//
//     private int currentWaveIndex = 0;
//     private int enemiesRemainingInWave;
//     private int totalEnemiesSpawned = 0;
//     private List<GameObject> activeEnemies = new List<GameObject>();
//     private Transform player;
//
//     private void Start()
//     {
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         StartCoroutine(SpawnWaves());
//     }
//
//     private IEnumerator SpawnWaves()
//     {
//         while (currentWaveIndex < waves.Count)
//         {
//             EnemyWave currentWave = waves[currentWaveIndex];
//             yield return new WaitForSeconds(currentWave.delayBeforeWave);
//
//             enemiesRemainingInWave = currentWave.count;
//
//             for (int i = 0; i < currentWave.count; i++)
//             {
//                 if (activeEnemies.Count >= maxActiveEnemies)
//                 {
//                     yield return new WaitUntil(() => activeEnemies.Count < maxActiveEnemies);
//                 }
//
//                 SpawnEnemy(currentWave);
//                 enemiesRemainingInWave--;
//                 totalEnemiesSpawned++;
//
//                 yield return new WaitForSeconds(currentWave.spawnInterval);
//             }
//
//             yield return new WaitUntil(() => enemiesRemainingInWave <= 0 && activeEnemies.Count == 0);
//             
//             currentWaveIndex++;
//             if (currentWaveIndex < waves.Count)
//             {
//                 yield return new WaitForSeconds(timeBetweenWaves);
//             }
//         }
//
//         Debug.Log("All waves completed!");
//     }
//
//     private void SpawnEnemy(EnemyWave wave)
//     {
//         Vector3 spawnPosition = GetSpawnPosition();
//         GameObject enemy = Instantiate(wave.enemyPrefab, spawnPosition, Quaternion.identity);
//         
//         TestenemyHealth enemyHealth = enemy.GetComponent<TestenemyHealth>();
//         if (enemyHealth != null)
//         {
//             float waveMultiplier = Mathf.Pow(healthMultiplierPerWave, currentWaveIndex);
//             // enemyHealth.(waveMultiplier);
//         }
//
//         if (Random.value <= wave.eliteChance)
//         {
//             EnemyLoot enemyLoot = enemy.GetComponent<EnemyLoot>();
//             if (enemyLoot != null)
//             {
//                 enemyLoot.enemyRank = EnemyRank.Elite;
//             }
//         }
//
//         enemyHealth.OnDeath += () => {
//             activeEnemies.Remove(enemy);
//         };
//
//         activeEnemies.Add(enemy);
//     }
//
//     private Vector3 GetSpawnPosition()
//     {
//         if (spawnInPlayerRadius)
//         {
//             Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
//             return player.position + new Vector3(randomCircle.x, 0, randomCircle.y);
//         }
//         else
//         {
//             return spawnPoints[Random.Range(0, spawnPoints.Length)].position;
//         }
//     }
//
//     private void OnDrawGizmosSelected()
//     {
//         if (spawnInPlayerRadius)
//         {
//             Gizmos.color = Color.red;
//             Gizmos.DrawWireSphere(transform.position, spawnRadius);
//         }
//         else
//         {
//             Gizmos.color = Color.blue;
//             foreach (var point in spawnPoints)
//             {
//                 Gizmos.DrawWireSphere(point.position, 0.5f);
//             }
//         }
//     }
// }