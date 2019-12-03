using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note2 : MonoBehaviour
{
    public Rigidbody myRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector3(0, 0, (200 / Conductor.instance.secPerBeat));
    }
}
