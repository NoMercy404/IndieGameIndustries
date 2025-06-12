using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<GameObject> weapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    // Key to switch weapons
    public KeyCode switchWeaponKey = KeyCode.Tab;

    void Start()
    {
        // Initialize by making sure only the current weapon is visible
        UpdateWeaponVisibility();
    }

    void Update()
    {
        // Switch weapons when key is pressed
        if (Input.GetKeyDown(switchWeaponKey) && weapons.Count > 1)
        {
            SwitchWeapon();
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        // Add the weapon to our list
        weapons.Add(weapon);

        // Set the current weapon to the newly added one
        currentWeaponIndex = weapons.Count - 1;

        // Deactivate all weapons except the current one
        UpdateWeaponVisibility();

        Debug.Log("Added weapon to inventory. Total weapons: " + weapons.Count);
    }

    private void SwitchWeapon()
    {
        // Move to the next weapon (loop back to start if needed)
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;

        // Update which weapon is visible
        UpdateWeaponVisibility();

        Debug.Log("Switched to weapon " + (currentWeaponIndex + 1) + " of " + weapons.Count);
    }

    private void UpdateWeaponVisibility()
    {
        // Hide all weapons
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                // Set active state based on whether this is the current weapon
                weapons[i].SetActive(i == currentWeaponIndex);

                // Also update the Weapon component's isActive property if it exists
                Weapon weaponComponent = weapons[i].GetComponent<Weapon>();
                if (weaponComponent != null)
                {
                    weaponComponent.SetActive(i == currentWeaponIndex);
                }

                Debug.Log("Weapon " + i + " visibility set to: " + (i == currentWeaponIndex));
            }
        }
    }
}
