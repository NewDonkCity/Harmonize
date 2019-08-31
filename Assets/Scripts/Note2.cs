using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note2 : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(-(1 / Conductor.instance.secPerBeat), myRigidbody.velocity.y);
    }
}
