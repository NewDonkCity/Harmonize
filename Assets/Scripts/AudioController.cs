using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //set these in the inspector!
    //public AudioSource master;
    //Define audio clips
    //public AudioClip[] clip;
    public AudioSource[] source;
    public KeyCode KeyUp;
    public KeyCode KeyDown;
    public int layer = 0;
    //public AudioSource[] slaves;

    void Awake()
    {
        for (int i = 0; i <= source.Length-1; i++)
        {
            //source[i] = AddAudio(clip[i], true, true, 0.2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j <= source.Length-1; j++)
        {
            //source[j].clip = clip[j];
            //master.PlayScheduled(AudioSettings.dspTime + 0.2);
            source[j].PlayScheduled(AudioSettings.dspTime + 0.2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyDown) && layer > 0)
        {
            //source[k].volume = source[k].volume - 0.01f;
            layer = layer - 1;
        }
        if (Input.GetKeyDown(KeyUp) && layer < 5)
        {
            //source[k].volume = source[k].volume + 0.01f;
            layer = layer + 1;
        }
        if (layer > 4)
            source[4].volume = 1;
        else
            source[4].volume = 0;
        if (layer > 3)
            source[3].volume = 1;
        else
            source[3].volume = 0;
        if (layer > 2)
            source[2].volume = 1;
        else
            source[2].volume = 0;
        if (layer > 1)
            source[1].volume = 1;
        else
            source[1].volume = 0;
        if (layer > 0)
            source[0].volume = 1;
        else
            source[0].volume = 0;
    }

    private IEnumerator SyncSources()
    {
        while (true)
        {
            foreach (var slave in source)
            {
                slave.timeSamples = source[0].timeSamples;
                yield return null;
            }
        }
    }
}
