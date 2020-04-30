using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telegraph : MonoBehaviour
{
    bool actLeft, actRight, actUp, actDown = false;
    public GameObject hitBoxUp, hitBoxLeft, hitBoxRight, hitBoxDown;
    public Image Background;
    public Color ImageColor;
    void Start()
    {
        hitBoxUp.gameObject.SetActive(false);
        hitBoxLeft.gameObject.SetActive(false);
        hitBoxRight.gameObject.SetActive(false);
        hitBoxDown.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //active = true;
        if (col.gameObject.tag == "HitLeft")
        {
            hitBoxLeft.gameObject.SetActive(true);
            //var color = hitBoxLeft.GetComponent<Renderer> ().material.color;
            //color.a = 0.5f;
            //pivot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (col.gameObject.tag == "HitRight")
        {
            hitBoxRight.gameObject.SetActive(true);
            //var color = hitBoxRight.GetComponent<Renderer> ().material.color;
            //color.a = 0.5f;
            //pivot.transform.rotation = Quaternion.Euler(0, 0, 270);
        }
        if (col.gameObject.tag == "HitUp")
        {
            hitBoxUp.gameObject.SetActive(true);
            //var color = hitBoxUp.GetComponent<Renderer> ().material.color;
            //color.a = 0.5f;
            //pivot.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (col.gameObject.tag == "HitDown")
        {
            hitBoxDown.gameObject.SetActive(true);
            //var color = hitBoxDown.GetComponent<Renderer> ().material.color;
            //color.a = 0.5f;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        //actLeft = false; actRight = false; actUp = false; actDown = false;
    }
}
