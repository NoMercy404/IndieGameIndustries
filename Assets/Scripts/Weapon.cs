<<<<<<< HEAD
using System;
=======
>>>>>>> eb0ecfd57a225b1671338a103b6b02a541f3d782
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
<<<<<<< HEAD
    int damage_min;
    int damage_max;
    int range;

    public Weapon(int damage_min, int damage_max, int range)
    {
        this.damage_min = damage_min;
        this.damage_max = damage_max;
        this.range = range;
    }

    void Start()
    {
        
=======
    public string weaponName;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Weapon " + weaponName + " created with damage " + damage);
>>>>>>> eb0ecfd57a225b1671338a103b6b02a541f3d782
    }

    // Update is called once per frame
    void Update()
    {
        
    }
<<<<<<< HEAD
    int getDamage()
    {
        return UnityEngine.Random.Range(damage_min, damage_max);
    }
=======
>>>>>>> eb0ecfd57a225b1671338a103b6b02a541f3d782
}
