using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public GameObject notes;
    public GameObject generationPoint;
    public float distanceBetween;
    private float notesWidth;
    public float compLoops = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
        //notesWidth = notes.GetComponent<CircleCollider2D>().radius * 2;
        //notesWidth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Conductor.instance.completedLoops > compLoops)
        {
            Instantiate(notes, generationPoint.transform.position, transform.rotation);
            compLoops = Conductor.instance.completedLoops;
        }
        */
    }

    IEnumerator Spawn()
    {
        Instantiate(notes, generationPoint.transform.position, transform.rotation);
        yield return new WaitForSeconds(5);
    }
}