﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayNote : MonoBehaviour
{
    SpriteRenderer sr;
    public KeyCode key;
    bool active = false;
    GameObject note;
    Color old;

    void Awake()
    {
        sr=GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        old=sr.color;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
            StartCoroutine(Pressed());
        if(Input.GetKeyDown(key) && active)
        {
            Destroy(note);
            AddScore();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        active = true;
        if(col.gameObject.tag == "Note")
            note=col.gameObject;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        active = false;
    }
    void AddScore()
    {
        PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")+100);
    }
    IEnumerator Pressed()
    {
        sr.color=new Color(0,0,0);
        yield return new WaitForSeconds(0.1f);
        sr.color=old;
    }
}
