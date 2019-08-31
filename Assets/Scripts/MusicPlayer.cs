using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    // Play an intro Clip followed by a loop
    [System.Serializable]
    public class AudioArray
    {
        public AudioSource[] audioSourceArray;
    }
    public AudioArray audioArray;

    public Dictionary<AudioSource, AudioClip> AudioArray2 = new Dictionary<AudioSource, AudioClip>();

    public AudioSource[] sources;
    public KeyCode key1, key2, key3;
    public double startTime;
    public int i = 0;

    void Start()
    {
        //StartCoroutine(playSound());
        startTime = AudioSettings.dspTime + 0.2;
    }

    void Update()
    {
        if (AudioSettings.dspTime > startTime - 0.2)
        {
            double duration = (double)sources[i].clip.samples / sources[i].clip.frequency;
            sources[i].PlayScheduled(startTime);
            startTime = startTime + duration;
            if (Input.GetKeyDown(key1))
            {
                sources[i].loop = false;
                i = 0;
            }
            if (Input.GetKeyDown(key2))
            {
                sources[i].loop = false;
                i = 1;
            }
            if (Input.GetKeyDown(key3))
            {
                sources[i].loop = false;
                i = 2;
            }
            else
                sources[i].loop = true;
        }
    }

    /*
    IEnumerator playSound()
    {
        yield return null;
        double startTime = AudioSettings.dspTime + 0.2;
        //1.Loop through each AudioClip
        for (int i = 0; i < sources.Length; i++)
        {
            int i = 0;
            double duration = (double)sources[i].clip.samples / sources[i].clip.frequency;
            sources[i].PlayScheduled(startTime);
            startTime = startTime + duration;
            if (Input.GetKeyDown(keyUp))
                i = 0;
            else if (Input.GetKeyDown(keyDown))
                i = 1;
            else if (i!=0)
                sources[i].loop = true;
            sources.Resize();
        }
    }
    */

    /*
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
    public static MusicPlayer instance;

    //Play an intro clip followed by a loop
    public AudioSource introAudioSource;
    public AudioSource loopAudioSource;
    public AudioSource endCueAudioSource;

    //Use two Audio Sources in an Array
    public AudioSource[] audioSourceArray;
    public AudioClip[] audioClipArray;
    public int toggle;

    public int nextClip;

    [HideInInspector] public double introDuration = 0;
    //[HideInInspector] public double duration = 0;
    [HideInInspector] public double nextStartTime = 0;

    public double timeSigNum;
    public double timeSigDenom = 4;

    public AudioClip engineStartClip;
    public AudioClip engineLoopClip;

    public AudioClip StartClip;
    public AudioClip LoopClip;

    public AudioSource audioSourceIntro;
    public AudioSource audioSourceLoop;
    private bool startedLoop;

    AudioClip otherClip;
    bool playNow1 = false;

    public float hSliderValue = 1.0f;
    public AudioClip[] clips;

    private bool playNow2 = false;
    private int cnt = 0;

    // declare Variable
    private bool OnPauseMenuSong = false;

    public AudioSource adSource;
    public AudioClip[] adClips;
    private readonly bool hasPlayed_1;
    private readonly bool hasPlayed_2;

    void Awake()
    {
        instance = this;
        bool hasPlayed_1 = false;
        bool hasPlayed_2 = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start the first Clip as soon as possible
        //AudioSource.PlayScheduled(AudioSettings.dspTime + 2);
        //audioSource.PlayScheduled(AudioSettings.dspTime + 0.5);

        // Saves the start time for later use
        double introDuration = (double)introAudioSource.clip.samples / introAudioSource.clip.frequency;

        AudioClip clipToPlay = audioClipArray[nextClip];

        // Checks how long the Clip will last and updates the Next Start Time with a new value
        double duration = (double)clipToPlay.samples / clipToPlay.frequency;

        double startTime = AudioSettings.dspTime + 0.2;
        introAudioSource.PlayScheduled(startTime);
        loopAudioSource.PlayScheduled(startTime + introDuration);

        //Start the music
        GetComponent<AudioSource>().Play();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        GetComponent<AudioSource>().loop = true;
        StartCoroutine(playEngineSound());

        StartCoroutine(playSound());

        StartCoroutine(playAudioSequentially());
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioSettings.dspTime > nextStartTime - 1)
        {
            AudioClip clipToPlay = audioClipArray[nextClip];

            //Loads the next clip to play and schedules when it will start
            audioSourceArray[toggle].clip = clipToPlay;
            audioSourceArray[toggle].PlayScheduled(nextStartTime);

            // Checks how long the Clip will last and updates the Next Start Time with a new value
            double duration = (double)clipToPlay.samples / clipToPlay.frequency;
            nextStartTime = nextStartTime + duration;

            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;

            // Increase the clip index number, reset if it runs out of clips
            nextClip = nextClip < audioClipArray.Length - 1 ? nextClip + 1 : 0;
        }

        // To calculate the semiquaver note length of a 110 bpm track in 4/4: 
        double noteLength = (60d / songBpm) / timeSigNum;

        // To calculate the semiquaver note length of a 110 bpm track in 4/4: 
        double barDuration = (60d / songBpm * timeSigNum) * (4 / timeSigDenom);

        // Get the current Time Elapsed
        //double timeElapsed = (double)AudioSource.timeSamples / AudioClip.Frequency;
        double timeElapsed = AudioSettings.dspTime - introDuration;

        // This line works out how far you are through the current bar
        double remainder = ((double)musicSource.timeSamples / musicSource.clip.frequency) % (barDuration);

        // This line works out when the next bar will occur
        double nextBarTime = AudioSettings.dspTime + barDuration - remainder;

        // Set the current Clip to end on the next bar
        musicSource.SetScheduledEndTime(nextBarTime);

        // Schedule an ending clip to start on the next bar
        endCueAudioSource.PlayScheduled(nextBarTime);

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

        if (playNow1)
        {
            // Assign the other clip  
            GetComponent<AudioSource>().clip = otherClip;
            GetComponent<AudioSource>().Play();
            playNow1 = false;
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            playNow2 = !playNow2;
            GetComponent<AudioSource>().Stop();
        }

        if (playNow2)
        {
            PlaySounds();
        }
    }

    void FixedUpdate()
    {
        if (!audioSourceIntro.isPlaying && !startedLoop)
        {
            audioSourceLoop.Play();
            Debug.Log("Done playing");
            startedLoop = true;
        }
    }

    IEnumerator playEngineSound()
    {
        GetComponent<AudioSource>().clip = engineStartClip;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        GetComponent<AudioSource>().clip = engineLoopClip;
        GetComponent<AudioSource>().Play();
    }

    IEnumerator playSound()
    {
        GetComponent<AudioSource>().clip = StartClip;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(StartClip.length);
        GetComponent<AudioSource>().clip = LoopClip;
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().loop = true;
    }

    void PlaySounds()
    {
        if (!GetComponent<AudioSource>().isPlaying && cnt < clips.Length)
        {
            GetComponent<AudioSource>().clip = clips[cnt];
            GetComponent<AudioSource>().volume = hSliderValue;
            GetComponent<AudioSource>().Play();
            cnt = cnt + 1;
        }
        if (cnt == clips.Length)
            cnt = 0;
    }

    IEnumerator playAudioSequentially()
    {
        yield return null;

        //1.Loop through each AudioClip
        for (int i = 0; i < adClips.Length; i++)
        {
            //2.Assign current AudioClip to audiosource
            adSource.clip = adClips[i];

            //3.Play Audio
            adSource.Play();

            //4.Wait for it to finish playing
            while (adSource.isPlaying)
            {
                yield return null;
            }
        }
    }
    */
}
