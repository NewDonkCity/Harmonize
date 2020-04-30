/**
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

    [HideInInspector] int nextClip;
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
    GameObject[] notes;
    public int[,,,] n = new int[3,4,16,4];

    float[] songBeats;
    int[] compLoops;
    public float[] loopAnalog;

    double nextTime;

    public GameObject[] notePrefabs; //size gets set in inspector! drag prefabs in there!

    double sampleRate = 0.0F;
    bool ticked = false;

    public Transform cam;

    public int i, k, l;

    public float speed;

    public GameObject[] character;

    void Awake()
    {
        instance = this;
        //character[2].gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        n[1,1,1,1]=1;n[1,1,2,2]=1;

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        nextStartTime = AudioSettings.dspTime + 0.2;
        nextBeatTime = 0;

        audioSourceArray[nextClip].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 1].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 2].PlayScheduled(nextStartTime);

        songBeats = new float[n.GetLength(2)];
        compLoops = new int[n.GetLength(2)];
        loopAnalog = new float[n.GetLength(2)];

        //nextTime = AudioSettings.dspTime + secPerBeat;

        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTime = startTick + secPerBeat;

        notes = new GameObject[n.GetLength(2)]; //makes sure they match 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(key1))
            nextClip = 0;
        if (Input.GetKey(key2))
            nextClip = 3;
        if (Input.GetKey(key3))
            nextClip = 6;
        if (AudioSettings.dspTime + 0.2 > nextStartTime)
        {

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
            audioSourceArray[currentClip].volume = 1;
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
        

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        for (int i = 0; i < n.GetLength(2); i++)
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
        
    }

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

        if (i <= n.GetLength(2) - 4)
        {
            for (int j = 0; j < n.GetLength(3); j++)
            {
                if (n[k,l,i,j] == 1);
                {
                    Debug.Log(n[k,l,i,j]);
                    //generationPoint.localPosition = new Vector3(UnityEngine.Random.Range(-10f, 0f), UnityEngine.Random.Range(0f, 10f), 20f);
                    notes[i] = Instantiate(notePrefabs[j], transform.position, transform.rotation) as GameObject;
                    notes[i].transform.parent = cam;
                    //startPosition = notes[i].transform.localPosition;
                    //speed = (endMarker.localPosition - startPosition).magnitude / (secPerBeat*1.7f);
                    //move.CrossFade("Left", 0.1f);
                    if (i > 3)
                        Destroy(notes[i - 4]);
                }
            }
            nextTime += secPerBeat;
            i++;
            if (i > n.GetLength(2) + 1)
                i = 0;
        }
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
    
}
**/