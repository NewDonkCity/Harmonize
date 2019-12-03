using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    public int index = 0;
    public int totalLevels = 3;
    public float yOffset = 1f;
    public GameObject[] character;
    int previousSelected;

    // Start is called before the first frame update
    void Start()
    {
        previousSelected = index;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (index < totalLevels - 1)
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (index > 0)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (previousSelected != index)
            {
                character[index].gameObject.SetActive(true);
                character[previousSelected].gameObject.SetActive(false);
                previousSelected = index;
            }
        }
    }
}