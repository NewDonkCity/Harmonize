using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor3 : MonoBehaviour
{
    public double nextTime;
    public double q;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        q = AudioSettings.dspTime;
        if (AudioSettings.dspTime >= nextTime)
        {
            nextTime += Conductor.instance.secPerBeat;
        }
    }
}
