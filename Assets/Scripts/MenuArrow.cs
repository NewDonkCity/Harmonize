using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuArrow : MonoBehaviour
{
    public int index = 0;
    public int totalLevels = 3, lowest = 0;
    public float yOffset = 1f;
    public GameObject[] character;
    int previousSelected;
    public float Pos1;

    public static MenuArrow instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousSelected = index;
        //character[2].gameObject.SetActive(false);
        Vector2 position = transform.position;
        Pos1 = position.y;
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector2 position = transform.position;
            position.y = Pos1;
            transform.position = position;
            if (Conductor.instance.swapped == true)
            {
                index = 2;
                lowest = 2;
            }
            else
            {
                index = 0;
                lowest = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (index < (lowest + totalLevels) - 1)
            {
                index++;
                Vector2 position = transform.position;
                position.y -= yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (index > lowest)
            {
                index--;
                Vector2 position = transform.position;
                position.y += yOffset;
                transform.position = position;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (character[index].activeSelf == false)
            {
                Conductor.instance.playerID = index;
                //if (Conductor.instance.swapped == false)
                {
                    character[index].gameObject.SetActive(true);
                    if (index == 0)
                        character[1].gameObject.SetActive(false);
                    if (index == 1)
                        character[0].gameObject.SetActive(false);
                }
                //else
                {
                    //character[index+2].gameObject.SetActive(true);
                    if (index == 2)
                        character[3].gameObject.SetActive(false);
                    if (index == 3)
                        character[2].gameObject.SetActive(false);
                }
                //Conductor.instance.playerID = index;
                //character[previousSelected].gameObject.SetActive(false);
                //previousSelected = index;
            }
            /**
            if (Conductor.instance.playerID <= 1)
            {
                if (character[index].activeSelf == false)
                {
                    Conductor.instance.playerID = index;
                    character[index].gameObject.SetActive(true);
                    //Conductor.instance.playerID = index;
                    character[previousSelected].gameObject.SetActive(false);
                    previousSelected = index;
                }
            }
            if (Conductor.instance.playerID >=2)
            {
                if (character[index-2].activeSelf == false)
                {
                    Conductor.instance.playerID = index;
                    int previousSelected2 = previousSelected - 2;
                    character[index-2].gameObject.SetActive(true);
                    //Conductor.instance.playerID = index;
                    character[previousSelected2].gameObject.SetActive(false);
                    previousSelected2 = index-2;
                }
            }
            **/
        }
    }
}