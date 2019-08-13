using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroyer : MonoBehaviour
{
    public GameObject noteDestructionPoint;

    // Start is called before the first frame update
    void Start()
    {
        noteDestructionPoint = GameObject.Find("NoteDestructionPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < noteDestructionPoint.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
