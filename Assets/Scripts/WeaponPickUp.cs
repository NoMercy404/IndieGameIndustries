using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private bool isInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpWeapon();
        }
    }

    void PickUpWeapon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform weaponHolder = player.transform.Find("WeaponHolder");

        // Jeśli gracz ma już broń — usuń starą broń
        if (weaponHolder.childCount > 0) 
        {
            foreach (Transform child in weaponHolder)
            {
                Destroy(child.gameObject);
            }
        }

        // PRZENIESIENIE broni do ręki gracza zamiast tworzenia nowej
        transform.SetParent(weaponHolder);
        transform.localPosition = Vector3.zero; // Ustawienie broni w centrum WeaponHoldera

        // WYŁĄCZENIE kolizji i fizyki po podniesieniu
        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().simulated = false;

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;
    }
}
