using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seven : MonoBehaviour
{
    public double nextStartTime = 0;
    public AudioSource[][] audioSourceArray;
    public AudioClip[] audioClipArray;
    public KeyCode key1, key2, key3;
    int toggle;
    public int nextClip;

    void Start()
    {
        nextStartTime = AudioSettings.dspTime + 0.2;
    }

    void Update()
    {
        if (Input.GetKey(key1))
            nextClip = 0;
        if (Input.GetKey(key2))
            nextClip = 1;
        if (Input.GetKey(key3))
            nextClip = 2;
        if (AudioSettings.dspTime > nextStartTime - 1)
        {

            AudioClip clipToPlay = audioClipArray[nextClip];

            for (int i = 0; i < audioSourceArray.Length; i++)
            {
                // Loads the next Clip to play and schedules when it will start
                audioSourceArray[toggle][i].clip = clipToPlay;
                audioSourceArray[toggle][i].PlayScheduled(nextStartTime);
            }

            // Checks how long the Clip will last and updates the Next Start Time with a new value
            double duration = (double)clipToPlay.samples / clipToPlay.frequency;
            nextStartTime = nextStartTime + duration;

            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;
        }
    }
}