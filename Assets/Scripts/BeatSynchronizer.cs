using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSynchronizer : MonoBehaviour
{
    public float bpm = 120f; // Tempo in beats per minute of the audio clip.
    public float startDelay = 1f; // Number of seconds to delay the start of audio playback.
    public delegate void AudioStartAction(double syncTime);
    public static event AudioStartAction OnAudioStart;
    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        double initTime = AudioSettings.dspTime;
        musicSource.PlayScheduled(initTime + startDelay);
        if (OnAudioStart != null)
        {
            OnAudioStart(initTime + startDelay);
        }
    }
}
