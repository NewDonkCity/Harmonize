using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGen : MonoBehaviour
{
    public float power = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
            power += Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            power += Time.deltaTime;
        }
        else if (Input.GetKey("w"))
        {
            power += Time.deltaTime;
        }
        else if (Input.GetKey("s"))
        {
            power += Time.deltaTime;
        }
        if (power > (Conductor.instance.secPerBeat + 0.2));
            power = 0;
    }
}
