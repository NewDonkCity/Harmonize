using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    public float beatPosition;

    //Conductor instance
    public static Conductor instance;

    //how many beats are shown in advance
    public int beatsShownInAdvance;

    //Use two Audio Sources in an Array
    public AudioSource[] audioSourceArray;

    public int nextClip = 0;
    [HideInInspector] int currentClip;

    public double nextStartTime = 0;
    public double nextBeatTime = 0;

    double duration;

    public KeyCode key1, key2, key3, keyUp, keyDown;

    public int layer = 0;

    // Transforms to act as start and end markers for the journey.
    public Transform endMarker;
    public Vector3 startPosition;
    public Vector3 currentPosition;

    public Transform generationPoint;

    //keep all the position-in-beats of notes in the song
    public GameObject[] notes;

    float[] songBeats;
    int[] compLoops;
    public float[] loopAnalog;

    double nextTime;

    public GameObject[] notePrefabs; //size gets set in inspector! drag prefabs in there!

    double sampleRate = 0.0F;
    bool ticked = false;
    public bool moving, swapped, choose;

    public Transform cam;

    public int i;

    public float speed;

    public GameObject[] character;

    public GameObject currentPos1, currentPos2;

    public Vector2 pos1, pos2, pos3;

    public GameObject menuUI;

    public int playerID;

    void Awake()
    {
        instance = this;
        //character[2].gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        nextStartTime = AudioSettings.dspTime + 0.2;
        nextBeatTime = 0;

        audioSourceArray[nextClip].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 1].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 2].PlayScheduled(nextStartTime);

        songBeats = new float[notePrefabs.Length];
        compLoops = new int[notePrefabs.Length];
        loopAnalog = new float[notePrefabs.Length];

        //nextTime = AudioSettings.dspTime + secPerBeat;

        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTime = startTick + secPerBeat;

        notes = new GameObject[notePrefabs.Length]; //makes sure they match 
        pos1 = currentPos1.transform.localPosition;
        pos2 = currentPos2.transform.localPosition;
        //currentPos3 = character[2].transform.localPosition;

        character[1].gameObject.SetActive(false);
        character[3].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            swapped = !swapped;
            moving = !moving;
            if (swapped)
                playerID = playerID + 2;
            else
                playerID = playerID - 2;
        }
        //if (Input.GetKeyDown(KeyCode.C))
            //menuUI.SetActive(false);
        if (Input.GetKey(key1))
            nextClip = 0;
        if (Input.GetKey(key2))
            nextClip = 3;
        if (Input.GetKey(key3))
            nextClip = 6;
        
        //if (Vector3.Distance(currentPos1.transform.localPosition, pos2) != 0 && moving == true)
        if (swapped == true && moving == true)
        {
            //currentPos1.transform.localPosition = pos2;
            //currentPos2.transform.localPosition = pos1;
            currentPos1.transform.localPosition = Vector2.MoveTowards(currentPos1.transform.localPosition, pos2, 0.5f);
            currentPos2.transform.localPosition = Vector2.MoveTowards(currentPos2.transform.localPosition, pos1, 0.5f);
            if (Vector3.Distance(currentPos1.transform.localPosition, pos2) == 0)
            {
                //playerID = playerID - 2;
                moving = false;
            }
        }
        if (swapped == false && moving == true)
        {
            //currentPos1.transform.localPosition = pos2;
            //currentPos2.transform.localPosition = pos1;
            currentPos1.transform.localPosition = Vector2.MoveTowards(currentPos1.transform.localPosition, pos1, 0.5f);
            currentPos2.transform.localPosition = Vector2.MoveTowards(currentPos2.transform.localPosition, pos2, 0.5f);
            if (Vector3.Distance(currentPos1.transform.localPosition, pos1) == 0)
            {
                //playerID = playerID + 2;
                moving = false;
            }
        }
        /**
        if (swapped == false && moving == true)
        {
            //currentPos1.transform.localPosition = pos2;
            //currentPos2.transform.localPosition = pos1;
            //swapped = true;
            playerID = playerID + 2;
            currentPos1.transform.localPosition = Vector2.MoveTowards(currentPos1.transform.localPosition, pos2, 0.5f);
            currentPos2.transform.localPosition = Vector2.MoveTowards(currentPos2.transform.localPosition, pos1, 0.5f);
            //if (Vector3.Distance(currentPos1.transform.localPosition, pos2) == 0)
                //moving = false;
        }
        if (swapped == true && moving == true)
        {
            //currentPos1.transform.localPosition = pos2;
            //currentPos2.transform.localPosition = pos1;
            //swapped = false;
            playerID = playerID - 2;
            currentPos1.transform.localPosition = Vector2.MoveTowards(currentPos1.transform.localPosition, pos1, 0.5f);
            currentPos2.transform.localPosition = Vector2.MoveTowards(currentPos2.transform.localPosition, pos2, 0.5f);
            //if (Vector3.Distance(currentPos1.transform.localPosition, pos1) == 0)
                //moving = false;
        }
        **/
        if (AudioSettings.dspTime + 0.2 > nextStartTime)
        {
            //currentPos1.transform.localPosition = pos2;
            //currentPos2.transform.localPosition = pos1;
            //if (nextClip >= 3 && nextClip <= 5)
            //if (nextClip <= 2)
            //swapped = !swapped;
            if (nextClip <= 2)
            {
                //playerID = 1;
                choose = true;
                //moving = true;
                menuUI.SetActive(true);
                /**
                // Keep a note of the time the movement started.
                startTime = Time.time;

                // Calculate the journey length.
                journeyLength = Vector3.Distance(startMarker.position, endMarker.position);

                // Distance moved equals elapsed time times speed..
                float distCovered = (Time.time - startTime) * speed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / journeyLength;

                // Set our position as a fraction of the distance between the markers.
                transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
                **/
            }
            else
            {
                choose = false;
                menuUI.SetActive(false);
            }
            if (nextClip == currentClip)
            {
                audioSourceArray[currentClip].loop = true;
                audioSourceArray[currentClip + 1].loop = true;
                audioSourceArray[currentClip + 2].loop = true;
            }
            else
            {
                // Loads the next Clip to play and schedules when it will start
                audioSourceArray[currentClip].loop = false;
                audioSourceArray[currentClip + 1].loop = false;
                audioSourceArray[currentClip + 2].loop = false;
                audioSourceArray[nextClip].PlayScheduled(nextStartTime);
                audioSourceArray[nextClip + 1].PlayScheduled(nextStartTime);
                audioSourceArray[nextClip + 2].PlayScheduled(nextStartTime);
            }

            // Checks how long the Clip will last and updates the Next Start Time with a new value
            duration = (double)audioSourceArray[currentClip].clip.samples / audioSourceArray[currentClip].clip.frequency;
            nextStartTime = nextStartTime + duration;

            currentClip = nextClip;
        }

        if (character[0].activeSelf)
            audioSourceArray[currentClip + 2].volume = 1;
        else
            audioSourceArray[currentClip + 2].volume = 0;
        if (character[1].activeSelf)
            audioSourceArray[currentClip + 1].volume = 1;
        else
            audioSourceArray[currentClip + 1].volume = 0;
        if (character[2].activeSelf)
        {
            audioSourceArray[currentClip].volume = 1;
            //playerID = 2;
        }
        else
            audioSourceArray[currentClip].volume = 0;

        /*
        if (Input.GetKeyDown(keyDown) && layer > 0)
        {
            layer = layer - 1;
        }
        if (Input.GetKeyDown(keyUp) && layer < 3)
        {
            layer = layer + 1;
        }
        if (layer > 2)
            audioSourceArray[currentClip + 2].volume = 1;
        else
            audioSourceArray[currentClip + 2].volume = 0;
        if (layer > 1)
            audioSourceArray[currentClip + 1].volume = 1;
        else
            audioSourceArray[currentClip + 1].volume = 0;
        if (layer > 0)
            audioSourceArray[currentClip].volume = 1;
        else
            audioSourceArray[currentClip].volume = 0;
        */

        /*
        // To calculate the semiquaver note length of a 110 bpm track in 4/4: 
        double noteLength = (60d / songBpm) / timeSigNum;

        // To calculate the semiquaver note length of a 110 bpm track in 4/4: 
        double barDuration = (60d / songBpm * timeSigNum) * (4 / timeSigDenom);

        // Get the current Time Elapsed
        //double timeElapsed = (double)AudioSource.timeSamples / AudioClip.Frequency;
        double timeElapsed = AudioSettings.dspTime - duration;

        // This line works out how far you are through the current bar
        double remainder = ((double)musicSource.timeSamples / musicSource.clip.frequency) % (barDuration);

        // This line works out when the next bar will occur
        double nextBarTime = AudioSettings.dspTime + barDuration - remainder;

        // Set the current Clip to end on the next bar
        musicSource.SetScheduledEndTime(nextBarTime);

        // Schedule an ending clip to start on the next bar
        //endCueAudioSource.PlayScheduled(nextBarTime);
        */

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        for (int i = 0; i < notes.Length; i++)
        {
            //determine how many beats since the song started
            songBeats[i] = (songPosition / secPerBeat) + i;

            //calculate the loop position
            if (songBeats[i] >= (compLoops[i] + 1) * beatsPerLoop)
                compLoops[i]++;
            loopAnalog[i] = (songBeats[i] - compLoops[i] * beatsPerLoop) / beatsPerLoop;
        }

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
        beatPosition = Mathf.Repeat(loopPositionInBeats/2, 1);

        secPerBeat = 60f / songBpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTime)
        {
            ticked = false;
            nextTime += secPerBeat;
        }

        /*
        if (i > 3 && i <= notePrefabs.Length)
        {
            
            //if (currentPosition != notes[i - 4].transform.localPosition)
            currentPosition = notes[i - 4].transform.localPosition;
            notes[i - 4].transform.localPosition = Vector3.MoveTowards(currentPosition, endMarker.localPosition, speed * Time.deltaTime);
            //notes[i - 4].transform.localPosition = Vector3.Lerp(currentPosition, endMarker.localPosition, beatPosition);
        }
        */
    }

    /**
    void LateUpdate()
    {
        if (!ticked && nextTime >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");
        }
    }
    // Just an example OnTick here
    void OnTick()
    {
        Debug.Log("Tick");

        if (i <= notePrefabs.Length - 4)
        {
            //generationPoint.localPosition = new Vector3(UnityEngine.Random.Range(-10f, 0f), UnityEngine.Random.Range(0f, 10f), 20f);
            //notes[i] = Instantiate(notePrefabs[i], transform.position, transform.rotation) as GameObject;
            //notes[i].transform.parent = cam;
            //startPosition = notes[i].transform.localPosition;
            //speed = (endMarker.localPosition - startPosition).magnitude / (secPerBeat*1.7f);
            //move.CrossFade("Left", 0.1f);
        }
        //if (i > 3)
            //Destroy(notes[i - 4]);

        nextTime += secPerBeat;
        i++;
        if (i > notePrefabs.Length + 1)
            i = 0;
    }

    /*
    void FixedUpdate()
    {
        secPerBeat = 60f / songBpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTime)
        {
            ticked = false;
            nextTime += secPerBeat;
        }

        if (i >= 1 && i <= notePrefabs.Length - 1)
        {
            currentPosition = notes[i - 1].transform.position;
            notes[i - 1].transform.position = Vector3.Lerp(currentPosition, endMarker.position, loopPositionInAnalog);
        }
    }
    */
}