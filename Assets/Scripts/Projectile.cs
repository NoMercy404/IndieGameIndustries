using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float distance = 0.5f;
    public int damage = 10;
    public LayerMask whatIsSolid;

    // Direction property for cursor-based shooting
    [HideInInspector]
    public Vector2 direction = Vector2.up;

    public GameObject destroyEffect;
    private bool hasHit = false;

    private void Start()
    {
        // Set a timer to destroy the projectile after its lifetime
        Invoke("DestroyProjectile", lifeTime);

        // Rotate the projectile to face the direction it's moving
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Remove any existing colliders that might be causing the green area
        Collider2D[] existingColliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in existingColliders)
        {
            Destroy(collider);
        }

        // Add a very thin BoxCollider2D that matches the arrow shape
        BoxCollider2D newCollider = gameObject.AddComponent<BoxCollider2D>();
        newCollider.size = new Vector2(0.1f, 0.5f); // Very thin collider
        newCollider.offset = Vector2.zero; // Center it on the arrow
        newCollider.isTrigger = true; // Make it a trigger

        Debug.Log("Projectile initialized with thin collider");
    }

    private void Update()
    {
        if (hasHit) return;

        // Move the projectile in the specified direction
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Cast a very short ray to detect only immediate collisions
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);

        // If we hit something
        if (hitInfo.collider != null)
        {
            // Check if we hit an enemy
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                // Try to get the Enemy component and apply damage
                Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplyDamage(damage);
                    Debug.Log("Hit enemy: " + enemy.name + " for " + damage + " damage");
                }

                // Destroy the projectile after hitting an enemy
                DestroyProjectile();
            }
            // If we hit a solid object (not an enemy)
            else if ((whatIsSolid.value & (1 << hitInfo.collider.gameObject.layer)) != 0)
            {
                DestroyProjectile();
            }
        }
    }

    // Handle direct collisions with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        // If we collided with an enemy, apply damage and destroy the projectile
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplyDamage(damage);
                Debug.Log("Hit enemy via trigger: " + collision.name);
            }

            // Destroy the projectile after hitting an enemy
            hasHit = true;
            DestroyProjectile();
        }
        // If we hit a solid object (not an enemy)
        else if ((whatIsSolid.value & (1 << collision.gameObject.layer)) != 0)
        {
            hasHit = true;
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
