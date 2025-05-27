using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;
    public LayerMask solidObjectsLayer;
    public LayerMask solidObjectsInvisibleLayer;

    public Transform weaponHolder;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 input;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleUpdate();
    }

    public void HandleUpdate()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("isRunning", input != Vector2.zero);

        if (input.x > 0)
        {
            spriteRenderer.flipX = false;

            if (weaponHolder != null)
                weaponHolder.localRotation = Quaternion.Euler(0f, 0f, -50f); // obrót broni w prawo
        }
        else if (input.x < 0)
        {
            spriteRenderer.flipX = true;

            if (weaponHolder != null)
                weaponHolder.localRotation = Quaternion.Euler(0f, 0f, 50f); // obrót broni w lewo
        }

        if (!isMoving && input != Vector2.zero)
        {
            Vector3 targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;

            if (isWalkable(targetPos))
                StartCoroutine(Move(targetPos));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Pozycja gracza: " + transform.position);
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        animator.SetTrigger("isRunning");
        isMoving = false;

        CheckForEncounters();
    }

    private bool isWalkable(Vector3 targetPos)
    {
        Vector2 origin = transform.position;
        Vector2 direction = (targetPos - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPos);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, solidObjectsLayer);
        if (hit.collider != null)
        {
            Debug.Log("Kolizja z: " + hit.collider.name + " na pozycji: " + hit.point);
        }
        return hit.collider == null;
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, solidObjectsLayer) != null)
        {
            if (Random.Range(1, 100) <= 25)
            {
                Debug.Log("Zadzialalo");
            }
        }
    }
}