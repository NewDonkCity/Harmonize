using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Four : MonoBehaviour
{

    public float hSliderValue = 1.0f;
    public AudioClip[] clips;
    public int hideGUItime = 5;

    private bool GUIhidden = false;
    private float mouseMoveTime = 0f;
    private bool playNow = false;
    private int cnt = 0;
    private Vector3 tmpMousePos;

    void Update()
    {
        DetectMouseMov();

        mouseMoveTime = mouseMoveTime + Time.deltaTime;

        if (mouseMoveTime > hideGUItime)
            GUIhidden = true;
        else
            GUIhidden = false;

        if (Input.GetKeyUp(KeyCode.M))
        {
            playNow = !playNow;
            GetComponent<AudioSource>().Stop();
        }

        if (playNow)
        {
            PlaySounds();
        }
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

    void OnGUI()
    {
        if (!GUIhidden)
        {
            GUI.Label(new Rect(25, Screen.height - 50, 100, 30), "Volume");
            hSliderValue = GUI.HorizontalSlider(new Rect(25, Screen.height - 25, 100, 30), hSliderValue, 0.0f, 1.0f);
        }
        Cursor.visible = !GUIhidden;
    }

    void DetectMouseMov()
    {
        if (tmpMousePos != Input.mousePosition)
        {
            mouseMoveTime = 0;
            tmpMousePos = Input.mousePosition;
        }
    }
}