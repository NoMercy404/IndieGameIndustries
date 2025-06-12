using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // Player health settings
    public float maxHealth = 100f;
    private float currentHealth;

    // Player damage settings
    public float meleeDamage = 10f;

    // UI elements
    public Slider healthSlider;
    public Text healthText;

    // Reference to the melee attack component
    private MeleeAttack meleeAttack;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Find and initialize the melee attack component
        meleeAttack = GetComponentInChildren<MeleeAttack>();
        if (meleeAttack != null)
        {
            // Pass the damage value to the melee attack component
            meleeAttack.damage = meleeDamage;
        }
        else
        {
            Debug.LogWarning("MeleeAttack component not found on player or its children!");
        }

        // Initialize UI elements
        SetupHealthUI();
    }

    void Update()
    {
        // Update UI if needed
        UpdateHealthUI();
    }

    // Setup the health UI elements
    private void SetupHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    // Update the health UI elements
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    // Apply damage to the player
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update UI
        UpdateHealthUI();

        // Check if player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Heal the player
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update UI
        UpdateHealthUI();
    }

    // Handle player death
    private void Die()
    {
        Debug.Log("Player has died!");

        // You can add game over logic here
        // For example:
        // GameManager.Instance.GameOver();

        // For now, just disable the player
        gameObject.SetActive(false);
    }

    // Update player's melee damage
    public void SetMeleeDamage(float newDamage)
    {
        meleeDamage = newDamage;

        // Update the MeleeAttack component
        if (meleeAttack != null)
        {
            meleeAttack.damage = meleeDamage;
        }
    }

    // Get current health percentage (0-1)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
