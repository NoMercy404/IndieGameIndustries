using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSystem : MonoBehaviour
{
    private bool playerDetection = false;
    public GameObject d_template; // dialog template
    public GameObject canva; // canvas dialogu
    public Text interactionText; // tekst od npc

    private DialogueUpdate dialogueUpdate;
    private bool isInDialogue = false; // flaga do rozmowy
    private List<GameObject> dialogueClones = new List<GameObject>(); // sklonowane dialogi

    // funkcja pobiera komponent templatki dialogu i ukrywa tekst od npc
    private void Start() {
        dialogueUpdate = canva.GetComponent<DialogueUpdate>();

        if (dialogueUpdate == null) {
            Debug.LogError("DialogueUpdate NIE został znaleziony w obiekcie canva! Upewnij się, że skrypt jest dodany.");
        }

        if (interactionText != null) {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update() {
        // jesli w zasiegu to wyswietl tekst
        if (playerDetection) {
            if (interactionText != null && !isInDialogue) {
                interactionText.gameObject.SetActive(true);
            }
        }
        else {
            if (interactionText != null) {
                interactionText.gameObject.SetActive(false);
            }
        }

        // rozpoczecie dialogu
        if (playerDetection && Input.GetKeyDown(KeyCode.F) && !isInDialogue) {
            Debug.Log("Rozpoczynam dialog");
            if (interactionText != null) {
                interactionText.gameObject.SetActive(false); // Ukrywamy tekst po rozpoczęciu rozmowy
            }
            StartDialogue();
        }

        // zakonczenie dialogu
        if (Input.GetKeyDown(KeyCode.Escape) && isInDialogue) {
            EndDialogue();
        }
    }

    private void StartDialogue() {
        isInDialogue = true;
        canva.SetActive(true);

        if (dialogueUpdate == null) {
            Debug.LogError("Brak komponentu DialogueUpdate! Sprawdź, czy został poprawnie dodany.");
            return;
        }

        // usuwanie starych dialogow
        foreach (var clone in dialogueClones) {
            clone.SetActive(false);
        }
        dialogueClones.Clear();

        // linie dialogowe
        NewDialogue("Hej, jestem ćpun.");
        NewDialogue("Chcesz trochę towaru?");
        NewDialogue("Jasne, oto mój towar:");

        // start systemu dialogowego
        dialogueUpdate.StartDialogue();
    }

    void NewDialogue(string text) {
        GameObject templateClone = Instantiate(d_template, canva.transform);
        templateClone.SetActive(false);
        templateClone.transform.SetParent(canva.transform, false);

        Transform textBoxTransform = templateClone.transform.Find("TextBox");
        if (textBoxTransform != null) {
            Text dialogueText = textBoxTransform.GetComponent<Text>();
            if (dialogueText != null) {
                dialogueText.text = text;
            }
            else {
                Debug.LogError("Nie znaleziono komponentu Text w TextBox!");
            }
        }
        else {
            Debug.LogError("Nie znaleziono obiektu TextBox w sklonowanym obiekcie!");
        }

        dialogueClones.Add(templateClone);
    }

    private void EndDialogue() {
        Debug.Log("Zakończenie dialogu");
        canva.SetActive(false);

        foreach (var clone in dialogueClones) {
            Destroy(clone);
        }
        dialogueClones.Clear();

        isInDialogue = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerDetection = true;
            Debug.Log("Gracz wszedł w zasięg NPC");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            playerDetection = false;
            Debug.Log("Gracz wyszedł z zasięgu NPC");

            // wyscie poza zasieg npc konczy rozmowe
            if (isInDialogue) {
                EndDialogue();
            }

            if (interactionText != null) {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
