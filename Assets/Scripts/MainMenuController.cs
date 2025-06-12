using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        bool currentlyActive = mainMenu.activeSelf;
        mainMenu.SetActive(!currentlyActive);
        Time.timeScale = !currentlyActive ? 0f : 1f;
    }

    public void OnStartClick()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}