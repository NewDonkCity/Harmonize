using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGenerator : MonoBehaviour
{
    public GameObject notes;
    public Transform generationPoint;
    //public Vector3 spawnPoint;
    public float distanceBetween;
    private float notesWidth;

    // Start is called before the first frame update
    void Start()
    {
        //notesWidth = notes.GetComponent<CircleCollider2D>().radius * 2;
        notesWidth = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generationPoint.position.x)
        {
            transform.position = new Vector3(transform.position.x + notesWidth + distanceBetween, transform.position.y, transform.position.z);
            Instantiate(notes, transform.position, transform.rotation);
        }
    }
}
