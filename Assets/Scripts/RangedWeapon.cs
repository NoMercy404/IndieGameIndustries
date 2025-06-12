using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    // Remove the constructor that's causing the error
    // public RangedWeapon(int damage_min, int damage_max, int range) : base(damage_min, damage_max, range)
    // {
    // }

    public float offset;
    public GameObject projectile;
    //public GameObject shotEffect;
    public Transform shotPoint;
    public Animator camAnim;
    private float timeBtwShots;
    public float startTimeBtwShots;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize any values here instead of in constructor
    }

    private void Update()
    {
        // Handles the weapon rotation
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        // transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                //Instantiate(shotEffect, shotPoint.position, Quaternion.identity);
                //camAnim.SetTrigger("shake");
                Instantiate(projectile, shotPoint.position, transform.rotation);
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    // You can add a public method to initialize the weapon if needed
    public void InitializeWeapon(int min, int max, int weaponRange)
    {
        damage_min = min;
        damage_max = max;
        range = weaponRange;
    }
}
