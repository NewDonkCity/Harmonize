using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBar : MonoBehaviour
{
    public float CurrentPts { get; set; }
    public float MaxPts { get; set; }
    public Slider hitbar;

    //HealthBar instance
    public static HitBar instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Resets health to full on game load
        MaxPts = 10f;
        CurrentPts = 0;

        hitbar.value = CalculatePts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPts(float ptsValue)
    {
        //Deduct the damage dealt from the character's health
        CurrentPts += ptsValue;
        hitbar.value = CalculatePts();
        //If the character is out of health, die
        if (CurrentPts >= 10)
            Charge();
    }

    float CalculatePts()
    {
        return CurrentPts / MaxPts;
    }

    void Charge()
    {
        CurrentPts = 10;
        Debug.Log("Attack charged.");
    }
}
