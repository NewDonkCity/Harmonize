using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour
{
    //public GameObject notes;
    public GameObject generationPoint;
    public GameObject noteDestructionPoint;
    public Rigidbody myRigidbody;
    public double x, y;
    public Vector3 z;

    // Start is called before the first frame update
    void Start()
    {
        //noteDestructionPoint = GameObject.Find("NoteDestructionPoint");
        //myRigidbody = notes.GetComponent<Rigidbody>();
        myRigidbody = GetComponent<Rigidbody>();
        //Instantiate(gameObject, generationPoint.transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        x = gameObject.transform.position.z;
        y = noteDestructionPoint.transform.position.z;
        myRigidbody.velocity = new Vector3(0, 0, (20 / Conductor.instance.secPerBeat));
        z = myRigidbody.velocity;
        if (gameObject.transform.position.z > noteDestructionPoint.transform.position.z)
        {
            Instantiate(gameObject, generationPoint.transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
