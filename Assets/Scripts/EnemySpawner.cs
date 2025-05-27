using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int enemiesPerWave = 3;
    public float spawnInterval = 10f;
    public float spawnDelay = 0.5f; // Delay between individual enemy spawns in a wave

    [Header("Movement Settings")]
    public float moveSpeed = 3f;

    private Transform playerTransform;

    void Start()
    {
        // Find the player in the scene
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure your player has the 'Player' tag.");
        }

        // Start spawning enemies
        StartCoroutine(SpawnEnemyWaves());
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab != null && playerTransform != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // Add EnemyMovement component to make the enemy follow the player
            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
            if (movement == null)
            {
                movement = enemy.AddComponent<EnemyMovement>();
            }

            movement.player = playerTransform;
            movement.moveSpeed = moveSpeed;
        }
    }
}
