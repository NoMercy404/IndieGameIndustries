using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHP = 100f;

  public static int counterKills = 0; // <--- DODAJ TO
    private float currentHP;

    public Slider HP;  // Referencja do paska HP
    public Text EnemyTag;  // Referencja do tekstu

    // Damage settings
    public float attackDamage = 10f;
    public float attackRange = 1f;  // Odleg�o�� 1 kratki
    public float attackCooldown = 1f;  // Czas mi�dzy atakami
    private float lastAttackTime;

    // Reference to player
    private Transform player;
    private PlayerStats playerStats;
  

    private void Start()
    {
        currentHP = maxHP;

        // Ustawienie domy�lnego imienia przeciwnika
        if (EnemyTag != null)
        {
            EnemyTag.text = "Wojownik"; // Mo�esz zmieni� na inne imi�
        }

        // Ustawienie pocz�tkowego HP
        if (HP != null)
        {
            HP.maxValue = maxHP;
            HP.value = maxHP;
        }

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStats = playerObj.GetComponent<PlayerStats>();
        }

        // Initialize attack timer
        lastAttackTime = -attackCooldown;  // Allow immediate attack
    }

    private void Update()
    {
        // Check if we can attack the player
        if (player != null && playerStats != null)
        {
            // Calculate distance to player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // If within attack range and cooldown has passed
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (player == null)
        {
            // Try to find player again if reference is lost
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerStats = playerObj.GetComponent<PlayerStats>();
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // Zapobiega warto�ciom ujemnym

        if (HP != null)
        {
            HP.value = currentHP;
        }

        if (currentHP <= 0)
        {
            Die();
            counterKills++; // Zwieksz licznik zabitych przeciwników
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Usuwa przeciwnika po �mierci
    }

    private void AttackPlayer()
    {
        // Apply damage to player
        playerStats.TakeDamage(attackDamage);

        // Visual feedback
        Debug.Log(gameObject.name + " attacks player for " + attackDamage + " damage!");

        // You could add attack animation or effects here
    }

    // Optional: Draw attack range in editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
