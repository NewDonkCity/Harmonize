/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteExtra : MonoBehaviour
{
    //Rigidbody2D rb;
    //public float speed;
    //bool called = false;

    /*
    public Vector3 SpawnPos;
    public Vector3 RemovePos;
    public int BeatsShownInAdvance;
    public int beatOfThisNote;
    public int songPosInBeats;

    public float beatTempo;
    //public bool hasStarted;

    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units/sec.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private int movingSpeed = 5;
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    Rigidbody notes;

    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //startpos = transform.position.x;
        //length = GetComponent<SpriteRenderer>().bounds.size.x;
        /*
        //beatTempo = beatTempo / 60f;

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position -= new Vector3(Conductor.instance.loopPositionInAnalog, 0f, 0f);
        //gameObject.transform.position = Quaternion.Euler(0, 0, Mathf.Lerp(0, 360, Conductor.instance.loopPositionInAnalog));
        /*
        if(PlayerPrefs.GetInt("Start")==1&&!called)
        {
            rb.velocity = new Vector2(0, -speed);
            called = true;
        }
        if(!hasStarted)
        {
            if(Input.anyKeyDown)
            {
                hasStarted = true;
            }
        }
        
        transform.position = Vector3.Lerp(SpawnPos,RemovePos,(BeatsShownInAdvance - (beatOfThisNote - songPosInBeats)) / BeatsShownInAdvance);
        
        //transform.position -= new Vector3((BeatsShownInAdvance - (beatOfThisNote - songPosInBeats)) / BeatsShownInAdvance, 0f, 0f);

        if (Conductor.instance.songPositionInBeats >= (Conductor.instance.completedLoops + 1) * Conductor.instance.beatsPerLoop)
        {
            // Instantiate the projectile at the position and rotation of this transform
            Rigidbody clone;
            clone = Instantiate(notes, transform.position, transform.rotation);
            // Give the cloned object an initial velocity along the current
            // object's Z axis
            clone.velocity = transform.TransformDirection(Vector3.forward * 10);
        }
        //transform.position += Vector3.left * Conductor.instance.secPerBeat;
        //transform.position -= new Vector3(1/Conductor.instance.secPerBeat, 0f, 0f);
        /*
        transform.position += Vector3.right * Time.deltaTime * movingSpeed;

        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position -= new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;

        // Distance moved = time * speed.
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed = current distance divided by total distance.
        float fracJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
    }
}
*/