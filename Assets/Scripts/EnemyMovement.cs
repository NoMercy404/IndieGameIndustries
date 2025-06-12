using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float knockbackForce = 10f;
    public float enemyRepelForce = 5f;

    // Layer settings
    public LayerMask solidObjectsLayer; // Assign this in the Inspector

    // Pathfinding settings
    public float obstacleDetectionDistance = 1.5f;
    public float pathUpdateInterval = 0.5f;
    public int maxPathfindingAttempts = 8;

    // Flag to prevent multiple collisions in a short time
    private bool canCollide = true;
    private float collisionCooldown = 0.5f;

    // Reference to the rigidbody for physics-based movement
    private Rigidbody2D rb;
    private Collider2D col;

    // Pathfinding variables
    private Vector2 currentPathDirection;
    private float pathTimer;
    private bool isPathBlocked = false;
    private List<Vector2> previousAttemptedDirections = new List<Vector2>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            // Add a rigidbody if one doesn't exist
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // Configure the rigidbody for proper collisions
        rb.gravityScale = 0; // Disable gravity for top-down movement
        rb.freezeRotation = true; // Prevent physics rotation
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Better collision detection
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Smoother movement

        // Make sure we have a collider
        col = GetComponent<Collider2D>();
        if (col == null)
        {
            // Add a circle collider if no collider exists
            col = gameObject.AddComponent<CircleCollider2D>();
        }

        // Ensure the collider is not a trigger
        col.isTrigger = false;

        // Keep the enemy in the Default layer (0)
        gameObject.layer = 0;

        // Initialize pathfinding
        currentPathDirection = Vector2.zero;
        pathTimer = 0;
    }

    void FixedUpdate() // Using FixedUpdate for physics
    {
        if (player != null)
        {
            // Calculate direct direction to the player
            Vector2 directDirection = ((Vector2)player.position - rb.position).normalized;

            // Update path direction periodically or when blocked
            pathTimer -= Time.fixedDeltaTime;
            if (pathTimer <= 0 || isPathBlocked)
            {
                UpdatePathDirection(directDirection);
                pathTimer = pathUpdateInterval;
            }

            // Apply the velocity based on current path direction
            if (currentPathDirection != Vector2.zero)
            {
                rb.velocity = currentPathDirection * moveSpeed;

                // Debug visualization
                Debug.DrawRay(transform.position, currentPathDirection * obstacleDetectionDistance,
                    isPathBlocked ? Color.red : Color.green);
            }
            else
            {
                // If no valid path, stop moving
                rb.velocity = Vector2.zero;
            }

            // Always make the enemy face the player regardless of movement direction
            float angle = Mathf.Atan2(directDirection.y, directDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            // If player reference is lost, try to find it again
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }

            // Stop movement if no player is found
            rb.velocity = Vector2.zero;
        }
    }

    // Update the path direction based on obstacles
    private void UpdatePathDirection(Vector2 directDirection)
    {
        // First try direct path to player
        if (!CheckForObstacles(directDirection))
        {
            currentPathDirection = directDirection;
            isPathBlocked = false;
            previousAttemptedDirections.Clear();
            return;
        }

        // If direct path is blocked, try to find an alternative path
        isPathBlocked = true;

        // Clear previous attempts if this is a new pathfinding sequence
        if (pathTimer <= 0)
        {
            previousAttemptedDirections.Clear();
        }

        // Add the direct direction to previous attempts
        previousAttemptedDirections.Add(directDirection);

        // Try to find a clear path using increasingly wider angles
        Vector2 bestDirection = FindBestAlternativePath(directDirection);

        if (bestDirection != Vector2.zero)
        {
            currentPathDirection = bestDirection;
            isPathBlocked = false;
        }
        else
        {
            // If no path is found, stop moving
            currentPathDirection = Vector2.zero;
        }
    }

    // Find the best alternative path when direct path is blocked
    private Vector2 FindBestAlternativePath(Vector2 originalDirection)
    {
        // Try several angles to find a clear path
        float[] testAngles = { 45f, -45f, 90f, -90f, 135f, -135f, 180f };

        // Try each angle in order
        foreach (float angle in testAngles)
        {
            // Rotate the original direction by the test angle
            Vector2 testDirection = RotateVector(originalDirection, angle);

            // Skip directions we've already tried recently
            bool alreadyTried = false;
            foreach (Vector2 prevDir in previousAttemptedDirections)
            {
                if (Vector2.Dot(testDirection, prevDir) > 0.9f) // Similar direction
                {
                    alreadyTried = true;
                    break;
                }
            }

            if (alreadyTried)
                continue;

            // Check if this direction is clear
            if (!CheckForObstacles(testDirection))
            {
                // Add this direction to previous attempts
                previousAttemptedDirections.Add(testDirection);

                // Limit the number of stored previous directions
                if (previousAttemptedDirections.Count > maxPathfindingAttempts)
                {
                    previousAttemptedDirections.RemoveAt(0);
                }

                return testDirection;
            }
        }

        // If we've tried too many directions without success, try a random direction
        if (previousAttemptedDirections.Count >= maxPathfindingAttempts)
        {
            Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            previousAttemptedDirections.Clear(); // Reset after trying random
            return randomDirection;
        }

        // If no clear path is found, return a zero vector (stop)
        return Vector2.zero;
    }

    // Check if there's an obstacle in the given direction
    private bool CheckForObstacles(Vector2 direction)
    {
        // Get the collider bounds
        float colliderExtent = 0;
        if (col is CircleCollider2D)
        {
            colliderExtent = ((CircleCollider2D)col).radius;
        }
        else if (col is BoxCollider2D)
        {
            BoxCollider2D boxCol = (BoxCollider2D)col;
            colliderExtent = Mathf.Max(boxCol.size.x, boxCol.size.y) / 2;
        }

        // Start the raycast from slightly in front of the collider
        Vector2 raycastOrigin = rb.position + direction * colliderExtent;

        // Cast a ray to detect obstacles
        RaycastHit2D hit = Physics2D.Raycast(
            raycastOrigin,
            direction,
            obstacleDetectionDistance,
            solidObjectsLayer
        );

        // Also check for other enemies in the way
        RaycastHit2D enemyHit = Physics2D.Raycast(
            raycastOrigin,
            direction,
            obstacleDetectionDistance,
            LayerMask.GetMask("Default") // Assuming enemies are on Default layer
        );

        if (enemyHit.collider != null &&
            (enemyHit.collider.GetComponent<Enemy>() != null ||
             enemyHit.collider.GetComponent<EnemyMovement>() != null) &&
            enemyHit.collider.gameObject != gameObject)
        {
            return true; // Consider other enemies as obstacles
        }

        // Debug visualization
        Debug.DrawRay(raycastOrigin, direction * obstacleDetectionDistance, hit ? Color.red : Color.green);

        return hit.collider != null;
    }

    // Helper method to rotate a vector by an angle in degrees
    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    // Handle collision with other objects
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug collision
        Debug.Log("Enemy collided with: " + collision.gameObject.name + " (Tag: " + collision.gameObject.tag + ", Layer: " + LayerMask.LayerToName(collision.gameObject.layer) + ")");

        // Check if we collided with the player
        if (collision.gameObject.CompareTag("Player") && canCollide)
        {
            // Apply knockback to player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            // Set cooldown to prevent multiple rapid collisions
            canCollide = false;
            Invoke("ResetCollisionCooldown", collisionCooldown);
        }
        // Check if we collided with another enemy
        else if (collision.gameObject.GetComponent<Enemy>() != null || collision.gameObject.GetComponent<EnemyMovement>() != null)
        {
            // Apply repelling force between enemies
            Vector2 repelDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(repelDirection * enemyRepelForce, ForceMode2D.Impulse);

            // Also apply force to the other enemy if it has a rigidbody
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                otherRb.AddForce(-repelDirection * enemyRepelForce, ForceMode2D.Impulse);
            }

            // Force a path recalculation
            isPathBlocked = true;
            pathTimer = 0;
        }
        // Check if we collided with a solid object (using the layer mask)
        else if (solidObjectsLayer == (solidObjectsLayer | (1 << collision.gameObject.layer)))
        {
            // Force a path recalculation
            isPathBlocked = true;
            pathTimer = 0;
        }
    }

    // Reset the collision cooldown
    private void ResetCollisionCooldown()
    {
        canCollide = true;
    }

    // For debugging collision issues
    private void OnDrawGizmos()
    {
        // Draw a visible radius around the enemy to show its collision area
        Gizmos.color = Color.red;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && col is CircleCollider2D)
        {
            CircleCollider2D circleCol = col as CircleCollider2D;
            Gizmos.DrawWireSphere(transform.position, circleCol.radius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }

        // Draw a line to the player if available
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }

        // Draw the obstacle detection range
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f); // Semi-transparent orange
        Gizmos.DrawWireSphere(transform.position, obstacleDetectionDistance);

        // Draw current path direction
        if (currentPathDirection != Vector2.zero)
        {
            Gizmos.color = isPathBlocked ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, currentPathDirection * obstacleDetectionDistance);
        }
    }
}
