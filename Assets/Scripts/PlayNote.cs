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
    public GameObject note2Left, note2Right, note2Up, note2Down, note3Left, note3Right, note3Up, note3Down;
    public Color old;
    public bool createMode;
    //public GameObject n;
    public GameObject hitBox;

    int type;

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
            if (Input.GetKey("z"))
            {
                if (Input.GetKeyDown(keyLeft))
                {
                    note = Instantiate(note2Left, transform.position, note2Left.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyRight))
                {
                    note = Instantiate(note2Right, transform.position, note2Right.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyUp))
                {
                    note = Instantiate(note2Up, transform.position, note2Up.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyDown))
                {
                    note = Instantiate(note2Down, transform.position, note2Down.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
            }
            else if (Input.GetKey("x"))
            {
                if (Input.GetKeyDown(keyLeft))
                {
                    note = Instantiate(note3Left, transform.position, note3Left.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyRight))
                {
                    note = Instantiate(note3Right, transform.position, note3Right.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyUp))
                {
                    note = Instantiate(note3Up, transform.position, note3Up.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyDown))
                {
                    note = Instantiate(note3Down, transform.position, note3Down.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
            }
            else
            {
                if (Input.GetKeyDown(keyLeft))
                {
                    note = Instantiate(noteLeft, transform.position, noteLeft.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyRight))
                {
                    note = Instantiate(noteRight, transform.position, noteRight.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyUp))
                {
                    note = Instantiate(noteUp, transform.position, noteUp.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
                if (Input.GetKeyDown(keyDown))
                {
                    note = Instantiate(noteDown, transform.position, noteDown.transform.rotation);
                    note.transform.SetParent(Conductor.instance.start.transform);
                }
            }
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
                if (!createMode)
                {
                    if (type == 0)
                        Destroy(note);
                    if (type == 1)
                        Destroy(note2);
                }
                HitBoss();
                //gm.GetComponent<GameManager>().AddStreak();
                //active = false;
            }
            if (Input.GetKeyDown(keyRight) && actRight)
            {
                HealthBar.instance.DealDamage(-6);
                if (!createMode)
                {
                    if (type == 0)
                        Destroy(note);
                    if (type == 1)
                        Destroy(note2);
                }
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                HitBoss();
                //active = false;
            }
            if (Input.GetKeyDown(keyUp) && actUp)
            {
                HealthBar.instance.DealDamage(-6);
                if (!createMode)
                {
                    if (type == 0)
                        Destroy(note);
                    if (type == 1)
                        Destroy(note2);
                }
                //gm.GetComponent<GameManager>().AddStreak();
                AddScore();
                HitBoss();
                //active = false;
            }
            if (Input.GetKeyDown(keyDown) && actDown)
            {
                HealthBar.instance.DealDamage(-6);
                if (!createMode)
                {
                    if (type == 0)
                        Destroy(note);
                    if (type == 1)
                        Destroy(note2);
                }
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
        if (!createMode)
        {
            //active = true;
            if (col.gameObject.tag == "NoteLeft")
            {
                type = 0;
                note = col.gameObject;
                actLeft = true;
            }
            if (col.gameObject.tag == "NoteRight")
            {
                type = 0;
                note = col.gameObject;
                actRight = true;
            }
            if (col.gameObject.tag == "NoteUp")
            {
                type = 0;
                note = col.gameObject;
                actUp = true;
            }
            if (col.gameObject.tag == "NoteDown")
            {
                type = 0;
                note = col.gameObject;
                actDown = true;
            }
            if (col.gameObject.tag == "HitLeft")
            {
                type = 1;
                note2 = col.gameObject;
                actLeft = true;
                warnLeft.gameObject.SetActive(false);
                hitLeft.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "HitRight")
            {
                type = 1;
                note2 = col.gameObject;
                actRight = true;
                warnRight.gameObject.SetActive(false);
                hitRight.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "HitUp")
            {
                type = 1;
                note2 = col.gameObject;
                actUp = true;
                warnUp.gameObject.SetActive(false);
                hitUp.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "HitDown")
            {
                type = 1;
                note2 = col.gameObject;
                actDown = true;
                warnDown.gameObject.SetActive(false);
                hitDown.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "ComboLeft")
            {
                type = 1;
                note2 = col.gameObject;
                actLeft = true;
                warnLeft.gameObject.SetActive(false);
                hitLeft.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "ComboRight")
            {
                type = 1;
                note2 = col.gameObject;
                actRight = true;
                warnRight.gameObject.SetActive(false);
                hitRight.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "ComboUp")
            {
                type = 1;
                note2 = col.gameObject;
                actUp = true;
                warnUp.gameObject.SetActive(false);
                hitUp.gameObject.SetActive(true);
            }
            if (col.gameObject.tag == "ComboDown")
            {
                type = 1;
                note2 = col.gameObject;
                actDown = true;
                warnDown.gameObject.SetActive(false);
                hitDown.gameObject.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        actLeft = false; actRight = false; actUp = false; actDown = false;
        if (!createMode)
        {
            if (type == 0)
                Destroy(note);
            if (type == 1)
            {
                if ((note2.gameObject.tag == "HitLeft" || note2.gameObject.tag == "ComboLeft") && Input.GetKey(KeyCode.D))
                {
                }
                else if ((note2.gameObject.tag == "HitRight" || note2.gameObject.tag == "ComboRight") && Input.GetKey(KeyCode.A))
                {
                }
                else if ((note2.gameObject.tag == "HitUp" || note2.gameObject.tag == "ComboUp") && Input.GetKey(KeyCode.S))
                {
                }
                else if ((note2.gameObject.tag == "HitDown" || note2.gameObject.tag == "ComboDown") && Input.GetKey(KeyCode.W))
                {
                }
                else
                {
                    HealthBar.instance.DealDamage(6);
                }
                Destroy(note2);
            }
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
