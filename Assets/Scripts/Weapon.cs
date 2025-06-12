using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName = "Default Weapon";
    public int damage_min = 10;
    public int damage_max = 20;
    public int range = 2;
    public Sprite weaponSprite;

    // Visual indicator when weapon is selected
    public bool isActive = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // If no sprite renderer, try to find it in children
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Set initial visual state
        UpdateVisualState();
    }

    // Set weapon active/inactive state
    public void SetActive(bool active)
    {
        isActive = active;

        // Also set the GameObject's active state to match
        gameObject.SetActive(active);

        UpdateVisualState();
    }

    // Update the visual appearance based on active state
    private void UpdateVisualState()
    {
        if (spriteRenderer != null)
        {
            // Make weapon fully visible when active, slightly transparent when inactive
            Color color = spriteRenderer.color;
            color.a = isActive ? 1.0f : 0.7f;
            spriteRenderer.color = color;
        }
    }

    // Get a random damage value within the weapon's damage range
    public int GetDamage()
    {
        return UnityEngine.Random.Range(damage_min, damage_max + 1);
    }
}
