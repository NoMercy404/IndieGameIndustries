using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float damage = 10f;
    private bool isAttacking = false;
    public float attackDuration = 0.2f; // krótszy czas
    private Collider2D currentTarget; // ostatni wróg, z którym się stykamy

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

        // Zadaj damage od razu, jeśli wróg już jest w triggerze
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

        // zapamiętaj ostatniego wroga, z którym się stykamy
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
        Enemy enemy = target.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            Debug.Log("Enemy detected: " + enemy.name);
            enemy.ApplyDamage(damage);
        }
        else
        {
            Debug.Log("No Enemy component found on: " + target.name);
        }
    }
}
