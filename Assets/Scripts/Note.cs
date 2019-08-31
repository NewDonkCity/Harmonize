using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform endMarker;
    public int beatCount;
    public Vector3 currentPosition;

    public GameObject notes;
    public Transform generationPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
}
