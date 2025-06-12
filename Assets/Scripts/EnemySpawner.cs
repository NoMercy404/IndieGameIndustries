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
            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            // IMPORTANT: Explicitly set the tag to "Enemy"
            enemy.tag = "Enemy";

            // Debug log to verify the tag is set
            Debug.Log("Spawned enemy with tag: " + enemy.tag);

            // Make sure the enemy has the Enemy component
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent == null)
            {
                enemyComponent = enemy.AddComponent<Enemy>();
                Debug.Log("Added Enemy component to spawned enemy");
            }
            else
            {
                Debug.Log("Enemy already has Enemy component");
            }

            // Add EnemyMovement component to make the enemy follow the player
            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
            if (movement == null)
            {
                movement = enemy.AddComponent<EnemyMovement>();
            }

            movement.player = playerTransform;
            movement.moveSpeed = moveSpeed;

            // Make sure the enemy has a collider
            Collider2D collider = enemy.GetComponent<Collider2D>();
            if (collider == null)
            {
                collider = enemy.AddComponent<CircleCollider2D>();
                Debug.Log("Added CircleCollider2D to spawned enemy");
            }

            // Make sure the collider is not a trigger
            collider.isTrigger = false;

            // Make sure the enemy is on the default layer (or whatever layer your projectiles check)
            enemy.layer = 0; // Default layer

            // Log the final setup
            Debug.Log("Enemy spawned: Tag=" + enemy.tag + ", Layer=" + LayerMask.LayerToName(enemy.layer) +
                      ", HasCollider=" + (collider != null) + ", IsTrigger=" + collider.isTrigger);
        }
    }
}
