using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    //public GameObject noteDestructionPoint;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector3(-(4f / Conductor.instance.secPerBeat), 0, 0);
        /**
        if (gameObject.transform.position.x < noteDestructionPoint.transform.position.x)
        {
            HealthBar.instance.DealDamage(6);
            Destroy(gameObject);
        }
        **/
    }
    /*
    // Transforms to act as start and end markers for the journey.
    public Transform endMarker;
    public Vector3 currentPosition;
    public float[] notePosition;

    //public GameObject notes;
    //public Transform generationPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        /*
        for (int i = 0; i < Conductor.instance.notes.Length; i++)
        {
            //if (nextIndex < notes.Length && notes[nextIndex] < songPositionInBeats + beatsShownInAdvance)
            if (Conductor.instance.notes[i] <= Conductor.instance.loopPositionInBeats)
            {
                notePosition[i] = Conductor.instance.notes[i] / Conductor.instance.beatsPerLoop;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(currentPosition, endMarker.position, Conductor.instance.loopPositionInAnalog);
        /*
        float step = Conductor.instance.noteSpeed * (float)AudioSettings.dspTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(currentPosition, endMarker.position, step);
        for (int i = 0; i < Conductor.instance.notes.Length; i++)
        {
            // Set our position as a fraction of the distance between the markers.
            //transform.position = Vector3.Lerp(currentPosition, endMarker.position, Conductor.instance.loopAnalog[Conductor.instance.notes[i]]);
            // Move our position a step closer to the target.
            //float step = Conductor.instance.noteSpeed * (float)AudioSettings.dspTime; // calculate distance to move
            //transform.position = Vector3.MoveTowards(currentPosition, endMarker.position, step);
            /*
            //if (nextIndex < notes.Length && notes[nextIndex] < songPositionInBeats + beatsShownInAdvance)
            if (Conductor.instance.notes[i] <= Conductor.instance.loopPositionInBeats)
            {
                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(currentPosition, endMarker.position, Conductor.instance.loopPositionInAnalog + notePosition[i]);
            }
        }
        //if (transform.position == endMarker.position)
        //if (Time.deltaTime >= 4)
        //Destroy(gameObject);
        /*
        if (Conductor.instance.completedLoops == beatCount - 1)
        {
            Instantiate(notes, generationPoint.position, transform.rotation);
        }
        if (Conductor.instance.completedLoops == beatCount)
        {
            // Set our position as a fraction of the distance between the markers.
            //Instantiate(notes, generationPoint.position, transform.rotation);
            transform.position = Vector3.Lerp(currentPosition, endMarker.position, Conductor.instance.loopPositionInAnalog);
        }
        if (Conductor.instance.completedLoops > beatCount)
        {
            Destroy(notes);
        }
    }
    */
}
