using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Animator move;
    //private Vector2 targetPos;
    //public float increment;

    //public float speed;

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
            //move.Play("Left");
            //targetPos = new Vector2(transform.position.x - increment, transform.position.y + increment);
            //transform.position = targetPos;
            move.CrossFade("Left",0.1f);
        }
        if (Input.GetKeyDown("d"))
        {
            //move.Play("Right");
            //targetPos = new Vector2(transform.position.x + increment, transform.position.y);
            //transform.position = targetPos;
            move.CrossFade("Right", 0.1f);
        }
        if (Input.GetKeyDown("w"))
        {
            //move.Play("Up");
            //targetPos = new Vector2(transform.position.x, transform.position.y + increment);
            //transform.position = targetPos;
            move.CrossFade("Up", 0.1f);
        }
        if (Input.GetKeyDown("s"))
        {
            //move.Play("Down");
            //targetPos = new Vector2(transform.position.x, transform.position.y - increment);
            //transform.position = targetPos;
            move.CrossFade("Down", 0.1f);
        }
    }
}
