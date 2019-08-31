using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public GameObject notes;
    public Transform generationPoint;
    public Transform endMarker;
    public int beatCount;
    public Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Conductor.instance.completedLoops == beatCount)
        {
            Instantiate(notes, generationPoint.position, transform.rotation);
        }
    }
}
