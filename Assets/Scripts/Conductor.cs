using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum AudioState { Off, Going1, Going2 };
[RequireComponent(typeof(AudioSource))]
public class Conductor : MonoBehaviour
{
    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    [HideInInspector] public float secPerBeat;

    //Current song position, in seconds
    [HideInInspector] public float songPosition;

    //Current song position, in beats
    [HideInInspector] public float songPositionInBeats;

    //How many seconds have passed since the song started
    [HideInInspector] public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    //public AudioSource musicSource;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    [HideInInspector] public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    [HideInInspector] public float loopPositionInAnalog;

    //Conductor instance
    public static Conductor instance;

    //how many beats are shown in advance
    //public float beatsShownInAdvance;

    //Use two Audio Sources in an Array
    public AudioSource[] audioSourceArray;

    //int toggle;

    int nextClip;
    int currentClip = 0;

    [HideInInspector] public double nextStartTime = 0;

    public double timeSigNum;
    public double timeSigDenom = 4;
    double duration;

    public KeyCode clipKey1, clipKey2, clipKey3, key1, key2, key3, keyUp, keyDown;

    public int layer = 0;

    // Transforms to act as start and end markers for the journey.
    public Transform endMarker;
    public int beatCount;
    public Vector3 currentPosition;

    public GameObject notes;
    public Transform generationPoint;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        nextStartTime = AudioSettings.dspTime + 0.2;

        audioSourceArray[nextClip].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 1].PlayScheduled(nextStartTime);
        audioSourceArray[nextClip + 2].PlayScheduled(nextStartTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(clipKey1))
            nextClip = 0;
        if (Input.GetKey(clipKey2))
            nextClip = 3;
        if (Input.GetKey(clipKey3))
            nextClip = 6;
        if (AudioSettings.dspTime + 0.2 > nextStartTime)
        {

            //AudioSource clipToPlay = audioSourceArray[nextClip];

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

            // Switches the toggle to use the other Audio Source next
            //toggle = 1 - toggle;
        }

        if (Input.GetKeyDown(keyDown) && layer > 0)
        {
            //source[k].volume = source[k].volume - 0.01f;
            layer = layer - 1;
        }
        if (Input.GetKeyDown(keyUp) && layer < 3)
        {
            //source[k].volume = source[k].volume + 0.01f;
            layer = layer + 1;
        }
        if (layer > 2)
            audioSourceArray[nextClip + 2].volume = 1;
        else
            audioSourceArray[nextClip + 2].volume = 0;
        if (layer > 1)
            audioSourceArray[nextClip + 1].volume = 1;
        else
            audioSourceArray[nextClip + 1].volume = 0;
        if (layer > 0)
            audioSourceArray[nextClip].volume = 1;
        else
            audioSourceArray[nextClip].volume = 0;
        if (Conductor.instance.completedLoops == beatCount - 1)
            Instantiate(notes, generationPoint.position, transform.rotation);
        if (Conductor.instance.completedLoops == beatCount)
        {
            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(transform.position, endMarker.position, Conductor.instance.loopPositionInAnalog);
        }
        if (Conductor.instance.completedLoops > beatCount)
        {
            Destroy(notes);
        }

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
        */

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    private IEnumerator SyncSources()
    {
        while (true)
        {
            foreach (var slave in audioSourceArray)
            {
                slave.timeSamples = audioSourceArray[0].timeSamples;
                yield return null;
            }
        }
    }
}
