// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {
//     public float moveSpeed;

//     private bool isMoving;
//     public LayerMask solidObjectsLayer;

//     public Transform weaponHolder;
//     private SpriteRenderer spriteRenderer;
//     private Animator animator;

//     private Vector2 input;

//     private void Awake()
//     {
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         animator = GetComponent<Animator>(); // Pobierz animator
//     }

//     private void Update()
//     {
//         HandleUpdate();
//     }

//     public static class GameData
//     {
//         public static Vector3 PlayerPosition;
//     }

//     public void HandleUpdate()
//     {
//         if (!isMoving)
//         {
//             input.x = Input.GetAxisRaw("Horizontal");
//             input.y = Input.GetAxisRaw("Vertical");

//             // Flipping postaci
//             if (input.x > 0)
//                 spriteRenderer.flipX = false;
//             else if (input.x < 0)
//                 spriteRenderer.flipX = true;

//             // Ustaw animację
//             animator.SetBool("isRunning", input != Vector2.zero); // <-- ZMIANA z "isMoving" na "isRunning"

//             if (input != Vector2.zero)
//             {
//                 var targetPos = transform.position;
//                 targetPos.x += input.x;
//                 targetPos.y += input.y;

//                 if (isWalkable(targetPos))
//                     StartCoroutine(Move(targetPos));
//             }
//             else
//             {
//                 animator.SetBool("isRunning", false); // <-- ZMIANA
//             }
//         }

//         if (Input.GetKeyDown(KeyCode.Z))
//         {
//             Debug.Log("Pozycja gracza: " + transform.position);
//         }
//     }

//     IEnumerator Move(Vector3 targetPos)
//     {
//         isMoving = true;

//         while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
//             yield return null;
//         }

//         transform.position = targetPos;
//         isMoving = false;

//         animator.SetBool("isRunning", false); // <-- ZMIANA
//         CheckForEncounters();
//     }

//     private bool isWalkable(Vector3 targetPos)
//     {
//         return Physics2D.OverlapCircle(targetPos, 0.3f, solidObjectsLayer) == null;
//     }

//     private void CheckForEncounters()
//     {
//         if (Physics2D.OverlapCircle(transform.position, 0.1f, solidObjectsLayer) != null)
//         {
//             if (Random.Range(1, 100) <= 25)
//             {
//                 Debug.Log("Zadzialalo");
//             }
//         }
//     }
// }
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

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

        // Ustaw animację natychmiast
        animator.SetBool("isRunning", input != Vector2.zero);

        // Flipping sprite'a
        if (input.x > 0)
            spriteRenderer.flipX = false;
        else if (input.x < 0)
            spriteRenderer.flipX = true;

        if (!isMoving)
        {
            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (isWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
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
        isMoving = false;
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
