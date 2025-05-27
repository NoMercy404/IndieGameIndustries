using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;

    void Update()
    {
        if (player != null)
        {
            // Calculate direction to the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move toward the player
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Optional: Make the enemy face the player
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            // If player reference is lost, try to find it again
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
