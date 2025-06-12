using UnityEngine;
using UnityEngine.UI; // Dla Legacy Text

public class KillCounterUI : MonoBehaviour
{
    public Text killText; // Przypisz w Inspectorze

    void Update()
    {
        killText.text = "Kills: " + Enemy.counterKills.ToString();
    }
}