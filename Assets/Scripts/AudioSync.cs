using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSync : MonoBehaviour
{
    //set these in the inspector!
    public AudioSource master;
    public AudioSource Slave;
    public AudioSource[] slaves;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Slave.timeSamples = master.timeSamples;
    }

    private IEnumerator SyncSources()
    {
        while (true)
        {
            foreach (var slave in slaves)
            {
                slave.timeSamples = master.timeSamples;
                yield return null;
            }
        }
    }
}