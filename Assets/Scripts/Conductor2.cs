using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor2 : MonoBehaviour
{
    /*
    public double nextTime;
    public GameObject note;
    public Transform generationPoint;
    public int i;
    public Vector3 currentPosition;
    public Transform endMarker;

    public GameObject[] notePrefabs; //size gets set in inspector! drag prefabs in there!
    */

    public double nextTime;
    public Vector3 currentPosition;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = AudioSettings.dspTime + Conductor.instance.secPerBeat;
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(SpawnNote());

        //if (AudioSettings.dspTime >= nextTime && i < notePrefabs.Length)
        if (AudioSettings.dspTime >= nextTime)
        {
            GameObject[] notes = new GameObject[Conductor.instance.notePrefabs.Length]; //makes sure they match length
            notes[i] = Instantiate(Conductor.instance.notePrefabs[i], Conductor.instance.generationPoint.position, transform.rotation) as GameObject;
            currentPosition = Conductor.instance.notePrefabs[i].transform.position;
            //var cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
            if (i >= 1)
                notes[i - 1].transform.position = Vector3.Lerp(currentPosition, Conductor.instance.endMarker.position, Conductor.instance.loopPositionInAnalog);
            if (i >= 2)
                Destroy(notes[i - 2]);
            nextTime += Conductor.instance.secPerBeat;
            i++;
            //nextTime = AudioSettings.dspTime + Conductor.instance.secPerBeat;
            /*
            counter++;
            if (counter == Conductor.instance.notes.Length)
                counter = 0;
            */
        }
    }
    /*
    IEnumerator SpawnNote()
    {
        if (AudioSettings.dspTime >= nextTime)
        {
            
            blocks = new GameObject[blockPrefabs.Length]; //makes sure they match length
            for (int i = 0; i < blockPrefabs.Length; i++)
                blocks[i] = Instantiate(blockPrefabs[i], generationPoint.position, transform.rotation) as GameObject;
            var cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
            nextTime += Conductor.instance.secPerBeat;
            yield return new WaitForSeconds(Conductor.instance.secPerBeat * 3);
            Destroy(cloneNote);
            counter++;
            if (counter == Conductor.instance.notes.Length)
                counter = 0;
            for (int i = 0; i < Conductor.instance.notes.Length - 1; i++)
            for (int i = 0; i < blockPrefabs.Length - 1; i++)
            {
                blocks = new GameObject[blockPrefabs.Length]; //makes sure they match length
                for (int i = 0; i < blockPrefabs.Length; i++)
                    blocks[i] = Instantiate(blockPrefabs[i], generationPoint.position, transform.rotation) as GameObject;
                //var cloneNote = Instantiate(note, generationPoint.position, transform.rotation);
                nextTime += Conductor.instance.secPerBeat;
                yield return new WaitForSeconds(Conductor.instance.secPerBeat * 3);
                Destroy(cloneNote);
                counter++;
                if (counter == Conductor.instance.notes.Length)
                    counter = 0;
            }
            
        }
        if (counter >= Conductor.instance.beatsShownInAdvance)
        //Destroy(note[counter - Conductor.instance.beatsShownInAdvance]);
        counter++;
        if (counter == Conductor.instance.notes.Length)
        counter = 0;

        var bombPos = transform.position + (transform.forward * 2);
        var cloneBomb = Instantiate(BombPrefab, bombPos, Quaternion.identity);
        yield WaitForSeconds(2);
        Explode();
        Destroy(cloneBomb);
    }
    */
}
