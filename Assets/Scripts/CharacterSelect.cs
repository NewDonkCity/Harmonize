using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public static bool menu = false;
    public GameObject menuUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            menu = !menu;
        }
        if (menu)
            Menu();
        else
            QuitMenu();
    }

    void QuitMenu()
    {
        menuUI.SetActive(false);
    }
    
    void Menu()
    {
        menuUI.SetActive(true);
    }
}
