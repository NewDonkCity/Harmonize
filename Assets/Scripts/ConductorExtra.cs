/*
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ConductorExtra;

public class ConductorExtra : MonoBehaviour
{
    bool called = false;

    //keep all the position-in-beats of notes in the song
    public float[] notes;

    //the index of the next note to be spawned
    [HideInInspector] public int nextIndex = 0;

    public GameObject musicNote;

    // Number of seconds to delay the start of audio playback.
    public double trackStartTime = 1f;

    //Conductor
    int crotchetsperbar = 8;

    public float crotchet;
    public float lasthit;// = 0.0f; // the last (snapped to beat) time spacebar was pressed
    public float actuallasthit;
    float nextbeattime = 0.0f;
    float nextbartime = 0.0f;
    public float offset = 0.2f; //positive means the song must be minussed!
    public float addoffset; //additional, per level offset.
    public static float offsetstatic = 0.40f;
    public static bool hasoffsetadjusted = false;
    public int beatnumber = 0;
    public int barnumber = 0;

    public delegate void AudioStartAction(double syncTime);
    public static event AudioStartAction OnAudioStart;

    /*
    //keep all the position-in-beats of notes in the song
    public float[] notes;

    //the index of the next note to be spawned
    public int nextIndex = 0;

    //how many beats are shown in advance
    public float beatsShownInAdvance;

    public GameObject musicNote;

    public bool startPlaying;

    public Note moveNote

    //int multiplier = 1;
    //int streak = 0;

    //int toggle;

    //public double timeSigNum;
    //public double timeSigDenom = 4;

    //public GameObject[] noteArray

    //public GameObject notes;
    //public Transform generationPoint;

    //public Note[] note;
    //public Note[] newNote;

    //public Transform notePoint;

//This structure contains all the information for this track
public struct Metadata
    {
        //Is the song's structure valid?
        public bool valid;

        //The Title, Subtitle and Artist for the song
        public string title;
        public string subtitle;
        public string artist;

        //The file paths for the related images and song media
        public string bannerPath;
        public string backgroundPath;
        public string musicPath;

        //The offset that the song starts at compared to the step info
        public float offset;

        //The start and length of the sample that is played when selecting a song
        public float sampleStart;
        public float sampleLength;

        //The bpm the song is played at
        public float bpm;

        //The note data for each difficulty, 
        //as well as a boolean to check that data for that difficulty exists
        public NoteData beginner;
        public bool beginnerExists;
        public NoteData easy;
        public bool easyExists;
        public NoteData medium;
        public bool mediumExists;
        public NoteData hard;
        public bool hardExists;
        public NoteData challenge;
        public bool challengeExists;
    }

    //This structure contains all the bars for a song at a single difficulty
    public struct NoteData
    {
        internal List<List> bars;

        public List> bars;
    }

    //This structure contains note information for a single 'row' of notes
    //Right now it's just a simple "Is there a note there or not"
    //But this could be modified and expanded to support numerous note types
    public struct Notes
    {
        public bool left;
        public bool right;
        public bool up;
        public bool down;
    }
    //Check if the file path is empty
    if (IsNullOrWhiteSpace(filePath))
    {
        //If so, Error and return invalid data
        Metadata tempMeta = new Metadata();
    tempMeta.valid = false;
        return tempMeta;
    }

//Create a boolean variable that we'll use to check whether
//we're currently parsing the notes or other metadata
private bool inNotes = false;
private Metadata songData = new Metadata();
    //Initialise Metadata
    //If it encounters any major errors during parsing, this is set to false and the song cannot be selected
    FontData.valid = true;
        songData.beginnerExists = false;
        songData.easyExists = false;
        songData.mediumExists = false;
        songData.hardExists = false;
        songData.challengeExists = false;
        
        //Collect the raw data from the sm file all at once
        private List fileData = File.ReadAllLines(filePath).ToList();
    
    //Get the file directory, and make sure it ends with either forward or backslash
    string fileDir = Path.GetDirectoryName(filePath);
        if (!fileDir.EndsWith("\\") && !fileDir.EndsWith("/"))
        {
            fileDir += "\\";
        }
        
        //Go through the file data
            for (int i = 0; i<fileData.Count; i++)
            {
                //Parse the data from the document
                string line = filePath[i].Trim();
                
                if (line.StartsWith("//"))
                {
                    //It's a comment, ignore it and go to the next line
                    continue;
                }
                else if (line.StartsWith("#"))
                {
                    //the # symbol denotes generic metadata for the song
                    string key = line.Substring(0, line.IndexOf(':')).Trim('#').Trim(':');
                    
                    switch (key.ToUpper())
                    {
                        case "TITLE":
                            songData.title = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            break;
                        case "SUBTITLE":
                            songData.subtitle = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            break;
                        case "ARTIST":
                            songData.artist = line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            break;
                        case "BANNER":
                            songData.bannerPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            break;
                        case "BACKGROUND":
                            songData.backgroundPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            break;
                        case "MUSIC":
                            songData.musicPath = fileDir + line.Substring(line.IndexOf(':')).Trim(':').Trim(';');
                            if (IsNullOrWhiteSpace(songData.musicPath) || !File.Exists(songData.musicPath))
                            {
                                //No music file found!
                                songData.musicPath = null;
                                songData.valid = false;
                            }
                            break;
                        case "OFFSET":
                            if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.offset))
                            {
                                //Error Parsing
                                songData.offset = 0.0f;
                            }
                            break;
                        case "SAMPLESTART":
                            if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleStart))
                            {
                                //Error Parsing
                                songData.sampleStart = 0.0f;
                            }
                            break;
                        case "SAMPLELENGTH":
                            if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.sampleLength))
                            {
                                //Error Parsing
                                songData.sampleLength = sampleLengthDefault;
                            }
                            break;
                        case "DISPLAYBPM":
                            if (!float.TryParse(line.Substring(line.IndexOf(':')).Trim(':').Trim(';'), out songData.bpm) || songData.bpm <= 0)
                            {
                                //Error Parsing - BPM not valid
                                songData.valid = false;
                                songData.bpm = 0.0f;
                            }
                            break;
                        case "NOTES":
                            inNotes = true;
                            break;
                        default:
                            break;
                    }
                }
    //If we're now parsing step data
            if (inNotes)
            {
                //We skip some feature we're not implementing for now
                if (line.ToLower().Contains("dance-double"))
                {
                    //And update the for loop we're in to adequately skip this section
                    for(int j = i; j<fileData.Count; j++)
                    {
                        if (fileData[j].Contains(";"))
                        {
                            i = j - 1;
                            break;
                        }
                    }
                }

                //Check if it's a difficulty
                if (line.ToLower().Contains("beginner") ||
                    line.ToLower().Contains("easy") ||
                    line.ToLower().Contains("medium") ||
                    line.ToLower().Contains("hard") ||
                    line.ToLower().Contains("challenge"))
                {
                    //And if it does have a difficulty declaration
                    //Then we're at the start of a 'step chart' 
                    string difficulty = line.Trim().Trim(':');
                }
            }

    //We update the parsing for loop to after the current step chart, and also record the note data along the way
    //We can then analyse the note data and parse it further
    List noteChart = new List();
                    for (int j = i; j<fileData.Count; j++)
                    {
                        string noteLine = fileData[j].Trim();
                        if private float beatWindow;
private BeatType beatMask;
private object musicSource;
private float loopTime;
private double trackStartTime;
private object beatObserver;
private object distance;
private object originalDistance;
private bool isInit;
private int barCount;
private object anim;
private object transform;
private int songTime;
private int previousFrameTime;
private int lastReportedPlayheadPosition;
private object originalArrowSpeed;
private object arrowSpeed;
private float barTime;

Song_Parser.Metadata FontData { get; private set; }

(noteLine.EndsWith(";"))
                        {
                            i = j - 1;
                            break;
                        }
                        else
                        {
                            noteChart.Add(noteLine);
                        }
                    }

                    //Here we determine what difficulty we're in, and begin parsing the accompanied note data
                    switch (difficulty.ToLower().Trim())
                    {
                        case "beginner":
                            songData.beginnerExists = true;
                            songData.beginner = ParseNotes(noteChart);
                            break;
                        case "easy":
                            songData.easyExists = true;
                            songData.easy = ParseNotes(noteChart);
                            break;
                        case "medium":
                            songData.mediumExists = true;
                            songData.medium = ParseNotes(noteChart);
                            break;
                        case "hard":
                            songData.hardExists = true;
                            songData.hard = ParseNotes(noteChart);
                            break;
                        case "challenge":
                            songData.challengeExists = true;
                            songData.challenge = ParseNotes(noteChart);
                            break;
                    }
                }
                if (line.EndsWith(";"))
                {
                    inNotes = false;
                }
            }
        }
    private IEnumerable<NoteData> ParseNotes(List notes)
{
    //We first instantiate our structures
    NoteData noteData = new NoteData();
    noteData.bars = new List<List>();

    //And then work through each line of the raw note data
    List bar = new List();
    for (int i = 0; i < notes.Count; i++)
    {
        //Based on different line properties we can determine what data that
        //line contains, such as a semicolon dictating the end of the note data
        //or a comma indicating the end of that bar
        string line = notes[i].Trim();

        if (line.StartsWith(";"))
        {
            break;
        }

        if (line.EndsWith(","))
        {
            noteData.bars.Add(bar);
            bar = new List();
        }
        else if (line.EndsWith(":"))
        {
            continue;
        }
        else if (line.Length >= 4)
        {
            //When we have a single 'note row' such as '0010' or '0110'
            //We check which columns will contain 'steps' and mark the appropriate flags
            Notes note = new Notes();
            note.left = false;
            note.down = false;
            note.up = false;
            note.right = false;


            if (line[0] != '0')
            {
                note.left = true;
            }
            if (line[1] != '0')
            {
                note.down = true;
            }
            if (line[2] != '0')
            {
                note.up = true;
            }
            if (line[3] != '0')
            {
                note.right = true;
            }

            //We then add this information to our current bar and continue until end
            bar.Add(note);
        }
    }

    yield return noteData;
    public void InitSteps(Song_Parser.Metadata newSongData,
                          Song_Parser.difficulties newDifficulty)
    {
        FontData = newSongData;
        isInit = true;

        //We estimate how many seconds a single 'bar' will be in the song
        //Using the bpm provided in the song data
        barTime = (60.0f / songData.bpm) * 4.0f;

        newDifficulty = newDifficulty;
        distance = originalDistance;

        //We then use the provided difficulty to determine how fast the arrows 
        //will be going
        switch (difficulty)
        {
            case Song_Parser.difficulties.beginner:
                arrowSpeed = 0.007f;
                noteData = songData.beginner;
                break;
            case Song_Parser.difficulties.easy:
                arrowSpeed = 0.009f;
                noteData = songData.easy;
                break;
            case Song_Parser.difficulties.medium:
                arrowSpeed = 0.011f;
                noteData = songData.medium;
                break;
            case Song_Parser.difficulties.hard:
                arrowSpeed = 0.013f;
                noteData = songData.hard;
                break;
            case Song_Parser.difficulties.challenge:
                arrowSpeed = 0.016f;
                noteData = songData.challenge;
                break;
            default:
                goto case Song_Parser.difficulties.easy;
        }

        //This variable is needed when we look at changing the speed of the song
        //with the variable BPM mechanic
        originalArrowSpeed = arrowSpeed;
    }

    void OnEnable()
    {
        BeatSynchronizer.OnAudioStart += () => { StartCoroutine(BeatCheck()); };
    }

    // Start is called before the first frame update
    void Start()
    {
        // Change when a scheduled Audio Source will play
        audioSource.SetScheduledStartTime(dspTime);

        // Stop a scheduled Audio Source
        AudioSource.Stop();

        // Stop a Scheduled Audio Source at an exact time
        audioSource.SetScheduledEndTime(dspTime);

        // Pause all Audio Sources
        AudioListener.pause = true;

        // Use an Audio Source when the Listener is paused
        AudioSource.ignoreListenerPause = true;    

        previousFrameTime = getTimer();
        lastReportedPlayheadPosition = 0;

        trackStartTime = AudioSettings.dspTime + 1;
        musicSource.PlayScheduled(trackStartTime);

        double initTime = AudioSettings.dspTime;
        musicSource.PlayScheduled(initTime + trackStartTime);
        OnAudioStart?.Invoke(initTime + trackStartTime);

        for (int i = 0; i < notes.Length; i++)
        {
            Instantiate(noteObjects[i], generationPoint.position, transform.rotation);
            //currentPosition[i] = noteObjects[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nextIndex < notes.Length && notes[nextIndex] < songPositionInBeats + beatsShownInAdvance)
        {
            //initialize the fields of the music note
            Instantiate(musicNote);

            nextIndex++;
        }

        var secondsSinceStartedPlaying = (musicSource.timeSamples / (double)musicSource.clip.samples) *
                                      musicSource.clip.length;
        songTime += getTimer() - previousFrameTime;
        previousFrameTime = getTimer();
        if (mySong.position != lastReportedPlayheadPosition)
        {
            songTime = (songTime + mySong.position) / 2;
            lastReportedPlayheadPosition = mySong.position;
        }


        // tip: don't use ui buttons for rhythm games. The callback is on the release...
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.currentSelectedGameObject != null &&
                        EventSystem.current.currentSelectedGameObject.CompareTag("HitBtn"))
                        TapTheBeat();
                }
            }
        }

        if ((beatObserver.beatMask & BeatType.DownBeat) == BeatType.DownBeat)
        {
            anim.SetTrigger("DownBeatTrigger");
        }
        if ((beatObserver.beatMask & BeatType.UpBeat) == BeatType.UpBeat)
        {
            transform.Rotate(Vector3.forward, 45f);
        }
        //If we're done initializing the rest of the world
        //And we havent gone through all the bars of the song yet
        if (isInit && barCount < noteData.bars.Count)
        {
            //We calculate the time offset using the s=d/t equation (t=d/s) 
            distance = originalDistance;
            float timeOffset = distance / arrowSpeed;

            //Get the current time through the song 
            songTimer = heartAudio.time;

            //If the current song time - the time Offset is greater than 
            //the time taken for all executed bars so far 
            //then it's time for us to spawn the next bar's notes 
            if (songTimer - timeOffset >= (barExecutedTime - barTime))
            {
                StartCoroutine(PlaceBar(noteData.bars[barCount++]));

                barExecutedTime += barTime;
            }
        }

        //StartCoroutine(playSound());

            //completedBeats = 0;

            //completedLoops = 0;

            // Switches the toggle to use the other Audio Source next
            //toggle = 1 - toggle;

            //currentPosition = transform.position;
            //notePosition = notes[beatCount] / beatsPerLoop;

        //AudioSource clipToPlay = audioSourceArray[nextClip];

        for (int i = 0; i < notes.Length; i++)
            {
                Instantiate(noteObjects[i], generationPoint.position, transform.rotation);
            }
        
        if (AudioSettings.dspTime + 0.2 > nextBeatTime)
        {
            //Instantiate(notes, generationPoint.position, transform.rotation);
            //transform.position = Vector3.Lerp(currentPosition, endMarker.position, loopPositionInAnalog);
            nextBeatTime = nextBeatTime + (secPerBeat*20);
        }
        //noteObjects[i].transform.position = Vector3.Lerp(generationPoint.position, endMarker.position, loopAnalog[i]);

        //if (nextIndex < notes.Length && notes[nextIndex] < songPositionInBeats + beatsShownInAdvance)
            if (notes[i] <= loopPositionInBeats)
            {
                //Note[] newNote = Instantiate(noteObjects[i], generationPoint.position, transform.rotation) as Note[];
                Instantiate(noteObjects[i], generationPoint.position, transform.rotation);
                noteObjects[i].transform.position = Vector3.Lerp(generationPoint.position, endMarker.position, loopAnalog[i]);
                // Set our position as a fraction of the distance between the markers.
                //+ (notes[i] / beatsPerLoop)
                notes[i] = 1000;
                //beatCount++;
                //if (beatCount == beatsPerLoop)
                //beatCount = 0;
            }
            //loopPositionInBeats + (i*0.1f)
            //noteObjects[i].transform.position = Vector3.Lerp(generationPoint.position, endMarker.position, loopPositionInBeats + (i/beatsPerLoop));
        
        for (int i = 0; i < notes.Length; i++)
            {
                //Instantiate(noteObjects[notes[i]], generationPoint.position, transform.rotation);
                //currentPosition[i] = noteObjects[i].transform.position;
            }
        for (int i = 0; i < notes.Length; i++)
        {
            // Set our position as a fraction of the distance between the markers.
            //noteObjects[i].transform.position = Vector3.Lerp(currentPosition[i], endMarker.position, loopPositionInAnalog);
        }

        if (songPositionInBeats >= completedBeats)
        {
            Instantiate(musicNote, generationPoint.position, transform.rotation);
            beatCount++;
            if (beatCount == noteArray.Length)
                beatCount = 0;
            completedBeats++;
        }
    }
    double GetTrackTime()
    {
        return AudioSettings.dspTime - trackStartTime;
    }

    #if UNITY_ANDROID
        const float MIN_TOUCH_LATENCY = 0.025f; // Samsung Galaxy Note Edge
    #elif UNITY_IOS
        const float MIN_TOUCH_LATENCY = 0.023f; // iPhone 6s Plus
    #else
        const float MIN_TOUCH_LATENCY = 0;
    #endif

    const float SHOULD_HIT_TIME = ????; // the supposed time the user should tap the screen/button
    const float TIMING_TOLERANCE = 0.02f; // just an example, the timeframe the user can hit 

    void UserAction()
    {
        float backwardTolerance = Time.unscaledDeltaTime + MIN_TOUCH_LATENCY;
        float actionTrackTime = GetTrackTime();

        float actionDelay = Mathf.Abs(SHOULD_HIT_TIME - actionTrackTime);

        if (actionTrackTime > SHOULD_HIT_TIME) // if the user input is later than the perfect hit time
            actionDelay -= backwardTolerance;

        if (actionDelay <= TIMING_TOLERANCE)
            Success();
        else
            Fail();
    }

    IEnumerator BeatCheck()
    {
        while (musicSource.isPlaying)
        {
            currentSample = (float)AudioSettings.dspTime * musicSource.clip.frequency;

            if (currentSample >= (nextBeatSample + sampleOffset))
            {
                foreach (GameObject obj in observers)
                {
                    obj.GetComponent<BeatObserver>().BeatNotify(beatType);
                }
                nextBeatSample += samplePeriod;
            }

            yield return new WaitForSeconds(loopTime / 1000f);
        }
    }

    public void BeatNotify(BeatType beatType)
    {
        beatMask |= beatType;
        StartCoroutine(WaitOnBeat(beatType));
    }

    IEnumerator WaitOnBeat(BeatType beatType)
    {
        yield return new WaitForSeconds(beatWindow / 1000f);
        beatMask ^= beatType;
    }

    def mouse_moved(ev)
        dt = Game.time() - last_time
        velocity = (ev.pos - last_pos) / dt
        last_time = dt
    end

    IEnumerator PlaceBar(List bar)
    {
        for (int i = 0; i < bar.Count; i++)
        {
            if (bar[i].left)
            {
                GameObject obj = (GameObject)Instantiate(leftArrow, new Vector3(leftArrowBack.transform.position.x, leftArrowBack.transform.position.y + distance, leftArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent().arrowBack = leftArrowBack;
            }
            if (bar[i].down)
            {
                GameObject obj = (GameObject)Instantiate(downArrow, new Vector3(downArrowBack.transform.position.x, downArrowBack.transform.position.y + distance, downArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent().arrowBack = downArrowBack;
            }
            if (bar[i].up)
            {
                GameObject obj = (GameObject)Instantiate(upArrow, new Vector3(upArrowBack.transform.position.x, upArrowBack.transform.position.y + distance, upArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent().arrowBack = upArrowBack;
            }
            if (bar[i].right)
            {
                GameObject obj = (GameObject)Instantiate(rightArrow, new Vector3(rightArrowBack.transform.position.x, rightArrowBack.transform.position.y + distance, rightArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent().arrowBack = rightArrowBack;
            }
            yield return new WaitForSeconds((barTime / bar.Count) - Time.deltaTime);
        }
    }
    /*
    public void NoteHit()
    {
        Debug.Log("Hit on time");
        currentScore += scorePerNote
    }

    public void NoteMissed()
    {
        Debug.Log("Missed Note");
    }
    public int GetScore();
    {
        return 100*multiplier;
    }

    IEnumerator playSound()
    {
        /*
        for (int i = 0; i < noteArray.Length-1; i++)
        {
            Instantiate(notes, generationPoint.position, transform.rotation);
            yield return new WaitForSeconds(secPerBeat * 8);
        }
        */

/*
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

int getTimer()
{
throw new NotImplementedException();
}

void TapTheBeat()
{
throw new NotImplementedException();
}

void StartCoroutine(IEnumerator enumerator)
{
throw new NotImplementedException();
}
*/