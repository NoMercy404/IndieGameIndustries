// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MeleeWeapon : Weapon
// {
//     public float swingSpeed = 1.0f; // Prędkość machania bronią
//     public float swingRange = 2.0f; // Zasięg ataku bronią
//     private bool isSwinging = false; // Flaga, aby sprawdzić, czy broń jest w trakcie machania

//     private float rotationSpeed = 30.0f; // Prędkość rotacji (stopni na sekundę)
//     private bool rotatingLeft = true; // Flaga kierunku rotacji (czy obraca się w lewo)

//     // Start is called before the first frame update
//     new void Start()
//     {
//         base.Start(); // Wywołanie metody Start() klasy bazowej (Weapon)
//     }

//     // Update is called once per frame
//     new void Update()
//     {
//         base.Update(); // Wywołanie metody Update() klasy bazowej (Weapon)
//         HandleSwing(); // Sprawdzanie, czy gracz chce machnąć bronią

//         // Obracanie broni w lewo/prawo w zakresie 180 stopni
//         RotateWeapon();
//     }

//     // Obsługuje machanie bronią
//     void HandleSwing()
//     {
//         if (Input.GetButtonDown("Fire1") && !isSwinging) // Przyciski myszy (np. lewy przycisk myszy)
//         {
//             StartCoroutine(SwingWeapon()); // Uruchamiamy korutynę, by wykonać machanie
//         }
//     }

//     // Korutyna dla animacji machania bronią
//     IEnumerator SwingWeapon()
//     {
//         isSwinging = true;
//         Debug.Log(weaponName + " is swinging!");

//         // Zasięg ataku: sprawdzamy, czy są przeciwnicy w okolicy machania
//         Collider[] hitEnemies = Physics.OverlapSphere(transform.position, swingRange);

//         foreach (Collider enemy in hitEnemies)
//         {
//             // Jeśli w trakcie machania trafiło się w przeciwnika, można zadawać obrażenia
//             Debug.Log("Hit " + enemy.name);
//         }

//         // Czas trwania machania bronią (symulacja animacji)
//         yield return new WaitForSeconds(swingSpeed);
//         isSwinging = false;
//     }

//     // Funkcja do obracania broni
//     void RotateWeapon()
//     {
//         float rotationStep = rotationSpeed * Time.deltaTime;

//         // Jeśli broń obraca się w lewo
//         if (rotatingLeft)
//         {
//             transform.Rotate(Vector3.up * -rotationStep); // Obracamy w lewo
//         }
//         else
//         {
//             transform.Rotate(Vector3.up * rotationStep); // Obracamy w prawo
//         }

//         // Sprawdzanie, czy osiągnęliśmy 180 stopni w lewo lub prawo
//         if (transform.eulerAngles.y <= 180 && rotatingLeft) 
//         {
//             rotatingLeft = false; // Zmieniamy kierunek na prawo
//         }
//         else if (transform.eulerAngles.y >= 360 && !rotatingLeft)
//         {
//             rotatingLeft = true; // Zmieniamy kierunek na lewo
//         }
//     }
// }