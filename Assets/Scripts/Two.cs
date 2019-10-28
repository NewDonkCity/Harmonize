using UnityEngine;
using System.Collections;

public class Two : MonoBehaviour
{
    public double bpm = 140.0F;

    double nextTick = 0.0F; // The next tick in dspTime
    double sampleRate = 0.0F;
    bool ticked = false;

    public double nextTime;
    public Vector3 currentPosition;
    public int i;
    public Transform generationPoint;
    public GameObject[] notePrefabs; //size gets set in inspector! drag prefabs in there!

    public GameObject[] notes;
    public GameObject cloneNote;

    void Start()
    {
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTick = startTick + (60.0 / bpm);

        nextTime = AudioSettings.dspTime + Conductor.instance.secPerBeat;

        notes = new GameObject[notePrefabs.Length]; //makes sure they match length
    }

    void LateUpdate()
    {
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");
        }
    }

    // Just an example OnTick here
    void OnTick()
    {
        Debug.Log("Tick");
        // GetComponent<AudioSource>().Play();

        if (i <= notePrefabs.Length - 2)
        {
            //notes = new GameObject[notePrefabs.Length]; //makes sure they match length
            notes[i] = Instantiate(notePrefabs[i], generationPoint.position, transform.rotation) as GameObject;
        }
        //var cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
        /*
        if (i >= 2 && i <= notePrefabs.Length - 1)
        {
            currentPosition = notes[i - 1].transform.position;
            notes[i - 1].transform.position = Vector3.Lerp(currentPosition, Conductor.instance.endMarker.position, Conductor.instance.loopPositionInAnalog);
        }
        */
        if (i >= 2)
            Destroy(notes[i - 2]);

        /*
        if (i == 0)
        {
            cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
            currentPosition = note.transform.position;
        }
        //var cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
        if (i >= 3)
            cloneNote.transform.position = Vector3.Lerp(currentPosition, Conductor.instance.endMarker.position, Conductor.instance.loopPositionInAnalog);
        if (i == 15)
            Destroy(cloneNote);
        */

        nextTime += Conductor.instance.secPerBeat;
        i++;
        if (i > 5)
            i = 0;
    }

    void FixedUpdate()
    {
        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }

        if (i >= 2 && i <= notePrefabs.Length - 1)
        {
            currentPosition = notes[i - 1].transform.position;
            notes[i - 1].transform.position = Vector3.Lerp(currentPosition, Conductor.instance.endMarker.position, Conductor.instance.loopPositionInAnalog);
        }
    }
}