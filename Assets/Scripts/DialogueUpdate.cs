using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUpdate : MonoBehaviour
{
    private int index = 1;
    private List<GameObject> dialogueBoxes = new List<GameObject>();

    // funkcja ktora usuwa wczesniejsze dialogi, dodaje do listy wszystkie dialogi, ukrywa je i wyswietla tylko pierwszy
    public void StartDialogue() {
        dialogueBoxes.Clear();

        foreach (Transform child in transform) {
            dialogueBoxes.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        if (dialogueBoxes.Count > 1) {
            index = 1;
            dialogueBoxes[index].SetActive(true);
        }
    }

    // po wcisnieciu spacji ukrywa aktualny dialog, przechodzi do nastepnego, jesli jest to ostatni (sklep) to usuwa mozliwosc klikania spacji, zamyka okno po skonczeniu dialogu, escape wychodzi z dialogu w dowolnym momencie
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueBoxes.Count > 1 && index < dialogueBoxes.Count - 1) {
            dialogueBoxes[index].SetActive(false);

            index++;

            if (index < dialogueBoxes.Count) {
                dialogueBoxes[index].SetActive(true);

                if (index == dialogueBoxes.Count - 1) {
                    ShopInterface(dialogueBoxes[index]);
                }
            }
            else {
                gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && dialogueBoxes.Count > 1) {
            gameObject.SetActive(false);
        }
    }

    //funkcja do wyswietlania sklepu
    private void ShopInterface(GameObject dialogueBox) {
        Transform placeholder1 = dialogueBox.transform.Find("Placeholder1");
        Transform shop = dialogueBox.transform.Find("Shop");
        if (shop != null && placeholder1 != null) {
            placeholder1.gameObject.SetActive(false); //chowamy tekst ze spacja
            shop.gameObject.SetActive(true); //wyswietlamy sklep
        }
    }
}
