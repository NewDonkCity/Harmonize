using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioState { Off, Going1, Going2 };
public class Eight : MonoBehaviour
{
    public AudioState state = AudioState.Off;

    public AudioSource thruster_Start;
    public AudioSource thruster_Going_1;
    public AudioSource thruster_Going_2;

    public double startDuration = 0;
    public double goingDuration = 0;
    public double nextStartTime = 0;

    public bool thrusterOn = false;
    public bool shuttingDown = false;

    public float thrusterVolume = 1.0f;

    void Start()
    {
        startDuration = (double)thruster_Start.clip.samples / thruster_Start.clip.frequency;
        goingDuration = (double)thruster_Going_1.clip.samples / thruster_Going_1.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        thrusterOn = Input.GetKey("Space");

        bool prepareNextAudio = (AudioSettings.dspTime > nextStartTime - 0.5);

        switch (state)
        {
            case AudioState.Off:
                if (thrusterOn)
                {
                    thrusterVolume = 1.0f;
                    SetAudioSourceVolume(thrusterVolume);

                    double startTime = AudioSettings.dspTime + 0.05;

                    thruster_Start.PlayScheduled(startTime);
                    nextStartTime = startTime + startDuration;
                    state = AudioState.Going1;
                }
                break;
            case AudioState.Going1:
                if (prepareNextAudio)
                {
                    thruster_Going_2.PlayScheduled(nextStartTime);
                    nextStartTime += goingDuration;
                    state = AudioState.Going2;
                }
                break;
            case AudioState.Going2:
                if (prepareNextAudio)
                {
                    thruster_Going_1.PlayScheduled(nextStartTime);
                    nextStartTime += goingDuration;
                    state = AudioState.Going1;
                }
                break;
        }

        if (state != AudioState.Off && !thrusterOn)
        {
            shuttingDown = true;
        }

        if (shuttingDown)
        {
            thrusterVolume -= 0.1f;

            if (thrusterVolume < 0)
            {
                thruster_Start.Stop();
                thruster_Going_1.Stop();
                thruster_Going_2.Stop();

                state = AudioState.Off;
                shuttingDown = false;
            }
            else
            {
                SetAudioSourceVolume(thrusterVolume);
            }
        }
    }

    void SetAudioSourceVolume(float volume)
    {
        thruster_Start.volume = volume;
        thruster_Going_1.volume = volume;
        thruster_Going_2.volume = volume;
    }
}