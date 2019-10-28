using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNote : MonoBehaviour
{
    SpriteRenderer sr;
    public KeyCode keyLeft, keyRight, keyUp, keyDown;
    bool actLeft, actRight, actUp, actDown, actUpDown = false;
    GameObject note, gm;
    Color old;
    //public bool createMode;
    //public GameObject n;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.Find("GameManager");
        old=sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyLeft | keyRight | keyUp | keyDown))
            StartCoroutine(Pressed());
        if (Input.GetKeyDown(keyLeft) && actLeft)
        {
            Destroy(note);
            //gm.GetComponent<GameManager>().AddStreak();
            AddScore();
            //active = false;
        }
        if (Input.GetKeyDown(keyRight) && actRight)
        {
            Destroy(note);
            //gm.GetComponent<GameManager>().AddStreak();
            AddScore();
            //active = false;
        }
        if (Input.GetKeyDown(keyUp) && actUp)
        {
            Destroy(note);
            //gm.GetComponent<GameManager>().AddStreak();
            AddScore();
            //active = false;
        }
        if (Input.GetKeyDown(keyDown) && actDown)
        {
            Destroy(note);
            //gm.GetComponent<GameManager>().AddStreak();
            AddScore();
            //active = false;
        }
        if (Input.GetKeyDown(keyUp) && Input.GetKeyDown(keyDown) && actUpDown)
        {
            Destroy(note);
            //gm.GetComponent<GameManager>().AddStreak();
            AddScore();
            //active = false;
        }

        /*
        if (createMode)
        {
            if (Input.GetKeyDown(key))
                Instantiate(n, transform.position, Quaternion.identity);
        }
        else
        {
            if (Input.GetKeyDown(key))
                StartCoroutine(Pressed());
            if (Input.GetKeyDown(key) && active)
            {
                Destroy(note);
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                active = false;
            }
            else if(Input.GetKeyDown(key) && !active)
            {
                //gm.GetComponent<GameManager>().ResetStreak();
            }
        }
        */
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //active = true;
        if (col.gameObject.tag == "NoteLeft")
        {
            note = col.gameObject;
            actLeft = true;
        }
        if (col.gameObject.tag == "NoteRight")
        {
            note = col.gameObject;
            actRight = true;
        }
        if (col.gameObject.tag == "NoteUp")
        {
            note = col.gameObject;
            actUp = true;
        }
        if (col.gameObject.tag == "NoteDown")
        {
            note = col.gameObject;
            actDown = true;
        }
        if (col.gameObject.tag == "NoteUpDown")
        {
            note = col.gameObject;
            actUpDown = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        actLeft = false; actRight = false; actUp = false; actDown = false; actUpDown = false;
        //gm.GetComponent<GameManager>().ResetStreak();
    }
    void AddScore()
    {
        //PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + gm.GetComponent<GameManager>().GetScore());
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 100);
    }
    IEnumerator Pressed()
    {
        sr.color=new Color(0,0,0);
        yield return new WaitForSeconds(0.05f);
        sr.color=old;
    }
}
