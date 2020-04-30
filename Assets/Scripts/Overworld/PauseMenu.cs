using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private static bool menu = false;
    public GameObject menuUI;

    // Update is called once per frame
    void Update()
    {
        // Menu UI toggle
        if (Input.GetKeyDown(KeyCode.M))
        {
            menu = !menu;
        }
        if (menu)
            Menu();
        else
            QuitMenu();
    }

    // Menu UI activates and deactivates, pausing and resuming time
    void QuitMenu()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
    }
    void Menu()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
