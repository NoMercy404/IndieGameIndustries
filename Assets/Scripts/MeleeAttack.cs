using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damage = 10f;
    private bool isAttacking = false;
    public float attackDuration = 0.2f;
    private Collider2D currentTarget;
    private PlayerStats playerStats;

    public Animator animator; // <-- Przypisz w Inspectorze

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        isAttacking = true;
        Debug.Log("Attack started!");

        // Wywołaj trigger animacji ataku mieczem
        if (animator != null)
            animator.SetTrigger("Triger"); 

        if (currentTarget != null)
        {
            ApplyDamageTo(currentTarget);
        }

        StartCoroutine(EndAttackAfterDelay());
    }

    private IEnumerator EndAttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        Debug.Log("Attack duration ended");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        currentTarget = other;

        if (isAttacking)
        {
            ApplyDamageTo(other);
            isAttacking = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == currentTarget)
        {
            currentTarget = null;
        }
    }

    private void ApplyDamageTo(Collider2D target)
    {
        if (playerStats != null)
        {
            damage = playerStats.meleeDamage;
        }

        Enemy enemy = target.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Enemy detected: " + enemy.name + ", applying damage: " + damage);
            enemy.ApplyDamage(damage);
        }
        else
        {
            Debug.Log("No Enemy component found on: " + target.name);
        }
    }
}