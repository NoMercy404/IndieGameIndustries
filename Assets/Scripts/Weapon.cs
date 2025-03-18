using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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

    }

    // Update is called once per frame
    void Update()
    {

    }
    int getDamage()
    {
        return UnityEngine.Random.Range(damage_min, damage_max);
    }
}