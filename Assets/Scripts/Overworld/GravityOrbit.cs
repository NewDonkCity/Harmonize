using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOrbit : MonoBehaviour
{
    // The enter that all objects orbit
    public float Gravity;
    // If the gravity of this section is only pushing the player downwards
    public bool FixedDirection;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GravityCtrl>())
        {
            // If this object has a gravity script, set this as the planet
            other.GetComponent<GravityCtrl>().Gravity = this.GetComponent<GravityOrbit>();
        }
    }
}
