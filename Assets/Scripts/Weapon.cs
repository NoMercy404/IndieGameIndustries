using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weapon " + weaponName + " created with damage " + damage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
