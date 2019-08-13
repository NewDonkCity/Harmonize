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
    [HideInInspector] public float secPerBeat;

    //Current song position, in seconds
    [HideInInspector] public float songPosition;

    //Current song position, in beats
    [HideInInspector] public float songPositionInBeats;

    //How many seconds have passed since the song started
    [HideInInspector] public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //the number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    [HideInInspector] public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    [HideInInspector] public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    [HideInInspector] public float loopPositionInAnalog;

    //Conductor instance
    public static Conductor instance;

    //how many beats are shown in advance
    public float beatsShownInAdvance;

    //bool called = false;

    /*
    //keep all the position-in-beats of notes in the song
    public float[] notes;

    //the index of the next note to be spawned
    [HideInInspector] public int nextIndex = 0;

    //how many beats are shown in advance
    public float beatsShownInAdvance;

    public GameObject musicNote;
    */

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //if(PlayerPrefs.GetInt("Start")==1&&called)

        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        /*
        if (nextIndex < notes.Length && notes[nextIndex] < songPositionInBeats + beatsShownInAdvance)
        {
            //initialize the fields of the music note
            Instantiate(musicNote);

            nextIndex++;
        }
        */

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }
}
