using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public float offset;
    public GameObject projectile;
    public Transform shotPoint;
    public Animator camAnim;

    // Shooting cooldown
    private float timeBtwShots;
    public float startTimeBtwShots = 0.2f;

    void Start()
    {
        // Initialize the shooting cooldown
        timeBtwShots = 0; // Allow immediate first shot

        // Make sure we have a shot point
        if (shotPoint == null)
        {
            // Create a shot point if none exists
            GameObject shotPointObj = new GameObject("ShotPoint");
            shotPointObj.transform.parent = transform;
            shotPointObj.transform.localPosition = new Vector3(0, 1, 0); // Adjust as needed
            shotPoint = shotPointObj.transform;

            Debug.LogWarning("No shot point assigned to RangedWeapon. Created one automatically.");
        }

        // Make sure we have a projectile prefab
        if (projectile == null)
        {
            Debug.LogError("No projectile prefab assigned to RangedWeapon!");
        }
    }

    private void Update()
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Calculate direction to mouse
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate weapon to face mouse cursor
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);

        // Debug visualization of shooting direction
        Debug.DrawRay(shotPoint.position, direction.normalized * 2f, Color.red);

        // Handle shooting
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0)) // Left mouse button
            {
                Shoot(direction.normalized);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void Shoot(Vector2 direction)
    {
        // Check if we have all required components
        if (projectile == null || shotPoint == null)
        {
            Debug.LogError("Missing projectile prefab or shot point!");
            return;
        }

        // Instantiate the projectile at the shot point position
        GameObject newProjectile = Instantiate(projectile, shotPoint.position, Quaternion.identity);

        // Get the projectile component
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            // Set the projectile's direction to the mouse cursor
            projectileComponent.direction = direction;

            // Set damage from the weapon
            projectileComponent.damage = Random.Range(damage_min, damage_max + 1);

            // Make sure the projectile has a collider that can detect enemies
            Collider2D collider = newProjectile.GetComponent<Collider2D>();
            if (collider == null)
            {
                collider = newProjectile.AddComponent<BoxCollider2D>();
                ((BoxCollider2D)collider).size = new Vector2(0.2f, 0.5f);
                collider.isTrigger = true;
            }
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Projectile component!");
        }

        // Log for debugging
        Debug.Log("Projectile fired from " + shotPoint.position + " toward cursor");

        // Apply camera shake if available
        if (camAnim != null)
        {
            camAnim.SetTrigger("shake");
        }
    }
}
