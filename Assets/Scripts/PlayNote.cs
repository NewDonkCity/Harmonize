using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNote : MonoBehaviour
{
    SpriteRenderer sr;
    public KeyCode keyLeft, keyRight, keyUp, keyDown;
    bool actLeft, actRight, actUp, actDown = false;
    public GameObject note, note2, noteLeft, noteRight, noteUp, noteDown, warnUp, warnLeft, warnRight, warnDown, hitLeft, hitRight, hitUp, hitDown, gm;
    public Color old;
    public bool createMode, createMode2;
    //public GameObject n;
    public GameObject hitBox;

    //PlayNote instance
    public static PlayNote instance;

    void Awake()
    {
        instance = this;
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //gm = GameObject.Find("GameManager");
        //warnLeft.gameObject.SetActive(false);
        //warnRight.gameObject.SetActive(false);
        //warnUp.gameObject.SetActive(false);
        //warnDown.gameObject.SetActive(false);
        hitLeft.gameObject.SetActive(false);
        hitRight.gameObject.SetActive(false);
        hitUp.gameObject.SetActive(false);
        hitDown.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (createMode)
        {
            if (Input.GetKeyDown(keyLeft))
                Instantiate(noteLeft, transform.position, noteLeft.transform.rotation);
            if (Input.GetKeyDown(keyRight))
                Instantiate(noteRight, transform.position, noteRight.transform.rotation);
            if (Input.GetKeyDown(keyUp))
                Instantiate(noteUp, transform.position, noteUp.transform.rotation);
            if (Input.GetKeyDown(keyDown))
                Instantiate(noteDown, transform.position, noteDown.transform.rotation);
        }
        if (createMode2)
        {
            if (Input.GetKeyDown(keyLeft))
                Instantiate(hitLeft, transform.position, noteLeft.transform.rotation);
            if (Input.GetKeyDown(keyRight))
                Instantiate(hitRight, transform.position, noteRight.transform.rotation);
            if (Input.GetKeyDown(keyUp))
                Instantiate(hitUp, transform.position, noteUp.transform.rotation);
            if (Input.GetKeyDown(keyDown))
                Instantiate(hitDown, transform.position, noteDown.transform.rotation);
        }
        else
        {
            if (Input.GetKeyDown(keyLeft))
                StartCoroutine(Pressed());
            if (Input.GetKeyDown(keyRight))
                StartCoroutine(Pressed());
            if (Input.GetKeyDown(keyUp))
                StartCoroutine(Pressed());
            if (Input.GetKeyDown(keyDown))
                StartCoroutine(Pressed());
            if (Input.GetKeyDown(keyLeft) && actLeft)
            {
                HealthBar.instance.DealDamage(-6);
                Destroy(note);
                Destroy(note2);
                HitBoss();
                //gm.GetComponent<GameManager>().AddStreak();
                //active = false;
            }
            if (Input.GetKeyDown(keyRight) && actRight)
            {
                HealthBar.instance.DealDamage(-6);
                Destroy(note);
                Destroy(note2);
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                HitBoss();
                //active = false;
            }
            if (Input.GetKeyDown(keyUp) && actUp)
            {
                HealthBar.instance.DealDamage(-6);
                Destroy(note);
                Destroy(note2);
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                HitBoss();
                //active = false;
            }
            if (Input.GetKeyDown(keyDown) && actDown)
            {
                HealthBar.instance.DealDamage(-6);
                Destroy(note);
                Destroy(note2);
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                HitBoss();
                //active = false;
            }
            /*
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
        if (col.gameObject.tag == "HitLeft")
        {
            note2 = col.gameObject;
            actLeft = true;
            warnLeft.gameObject.SetActive(false);
            hitLeft.gameObject.SetActive(true);
        }
        if (col.gameObject.tag == "HitRight")
        {
            note2 = col.gameObject;
            actRight = true;
            warnRight.gameObject.SetActive(false);
            hitRight.gameObject.SetActive(true);
        }
        if (col.gameObject.tag == "HitUp")
        {
            note2 = col.gameObject;
            actUp = true;
            warnUp.gameObject.SetActive(false);
            hitUp.gameObject.SetActive(true);
        }
        if (col.gameObject.tag == "HitDown")
        {
            note2 = col.gameObject;
            actDown = true;
            warnDown.gameObject.SetActive(false);
            hitDown.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        actLeft = false; actRight = false; actUp = false; actDown = false;
        if (!createMode)
            Destroy(note);
        if (!createMode2)
        {
            if (note2.gameObject.tag == "HitLeft" && Input.GetKey(KeyCode.D))
            {
            }
            else if (note2.gameObject.tag == "HitRight" && Input.GetKey(KeyCode.A))
            {
            }
            else if (note2.gameObject.tag == "HitUp" && Input.GetKey(KeyCode.S))
            {
            }
            else if (note2.gameObject.tag == "HitDown" && Input.GetKey(KeyCode.W))
            {
            }
            else
            {
                HealthBar.instance.DealDamage(6);
            }
            Destroy(note2);
            hitLeft.gameObject.SetActive(false);
            hitRight.gameObject.SetActive(false);
            hitUp.gameObject.SetActive(false);
            hitDown.gameObject.SetActive(false);
            //gm.GetComponent<GameManager>().ResetStreak();
        }
    }
    void AddScore()
    {
        //PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + gm.GetComponent<GameManager>().GetScore());
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 100);
        HealthBar.instance.DealDamage(-1);
        HitBar.instance.AddPts(1);
    }
    void HitBoss()
    {
        EnemyHealthBar.instance.DealDamage(1+Dash.instance.power);
    }
    IEnumerator Pressed()
    {
        sr.color=new Color(0,0,0);
        yield return new WaitForSeconds(0.05f);
        sr.color=old;
    }
}
