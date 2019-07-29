using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Animator move;
    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            move.Play("Left");
        }
        if (Input.GetKeyDown("d"))
        {
            move.Play("Right");
        }
        if (Input.GetKeyDown("w"))
        {
            move.Play("Up");
        }
        if (Input.GetKeyDown("s"))
        {
            move.Play("Down");
        }
    }
}
