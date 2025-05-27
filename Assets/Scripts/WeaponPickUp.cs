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

    if (weaponHolder == null)
    {
        Debug.LogError("Nie znaleziono WeaponHolder!");
        return;
    }

    // Usuń starą broń
    foreach (Transform child in weaponHolder)
    {
        Destroy(child.gameObject);
    }

    // Resetuj skalę i rotację broni
    transform.SetParent(weaponHolder);
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.Euler(0f, 0f, 90f); // Dopasuj do ręki
    transform.localScale = Vector3.one;

    Debug.Log("Broń podniesiona!");
}

}
