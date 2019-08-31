using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Stat : MonoBehaviour
{
    [SerializeField] private BarScript bar;

    [SerializeField] private float maxVal;

    [SerializeField] public float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }
        set
        {
            this.currentVal = Mathf.Clamp(value,0,MaxVal);
            bar.Value = currentVal;
        }
    }

    private float MaxVal
    {
        get
        {
            return maxVal;
        }
        set
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
