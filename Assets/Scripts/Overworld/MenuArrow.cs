using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    [SerializeField]
    int index = 0;
    int totalLevels = 3;
    float yOffset = 180f;

    // Update is called once per frame
    void Update()
    {
        // Menu selection using arrow keys
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (index < totalLevels - 1)
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (index > 0)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }
    }
}
