using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    //public GameObject player;
    //public Animator move;
    //private Vector2 targetPos;
    //public float increment;

    //public float speed;
    //public float chargeSpeed = 20;
    public float power;
    public bool buttonHeldDown;
    public Vector2 currentPos;
    public float startTime;
    public float secPerBeat;
    public int playerID;

    public GameObject home;

    //HealthBar instance
    public static Dash instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / Conductor.instance.songBpm;
        //move = GetComponent<Animator>();
        currentPos = home.transform.localPosition;
    }

    void Update()
    {
        if (playerID == Conductor.instance.playerID)
            Dashing();
        //if (Conductor.instance.playerID <= 1)
        /**
        if (Conductor.instance.swapped)
        {
            //playerID = Conductor.instance.playerID;
            if (playerID == MenuArrow.instance.index)
                Dashing();
        }
        //if (Conductor.instance.playerID >= 2)
        else
        {
            //playerID = Conductor.instance.playerID - 2;
            if (playerID == MenuArrow.instance.index + 2)
                Dashing();
        }
        **/
    }

    // Update is called once per frame
    void Dashing()
    {
        StartCoroutine(Waiting());
        if (power <= secPerBeat || buttonHeldDown == false)
            power = 0f;
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown("w") || Input.GetKeyDown("s"))
        {
            startTime = Time.time;
        }
        if (Input.GetKey("a") && power <= secPerBeat)
        {
            if (power > secPerBeat)
                power = 0f;
            else
                power = (Time.time - startTime);
        }
        else if (Input.GetKey("d") && power <= (secPerBeat + 0.2f))
        {
            if (power > secPerBeat)
                power = 0f;
            else
                power = (Time.time - startTime);
        }
        else if (Input.GetKey("w") && power <= (secPerBeat + 0.2f))
        {
            if (power > secPerBeat)
                power = 0f;
            else
                power = (Time.time - startTime);
        }
        else if (Input.GetKey("s") && power <= (secPerBeat + 0.2f))
        {
            if (power > secPerBeat)
                power = 0f;
            else
                power = (Time.time - startTime);
        }
    }

    IEnumerator Waiting()
    {
        if (Input.GetKey("a") && buttonHeldDown == false)
        {
            buttonHeldDown = true;
            transform.Translate(new Vector2(-2,0));
            while (Input.GetKey("a"))
                yield return new WaitForSeconds(0);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentPos, 2.1f);
            buttonHeldDown = false;
        }
        else if (Input.GetKey("d") && buttonHeldDown == false)
        {
            buttonHeldDown = true;
            transform.Translate(new Vector2(2,0));
            while (Input.GetKey("d"))
                yield return new WaitForSeconds(0);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentPos, 2.1f);
            buttonHeldDown = false;
        }
        else if (Input.GetKey("w") && buttonHeldDown == false)
        {
            buttonHeldDown = true;
            transform.Translate(new Vector2(0,2));
            while (Input.GetKey("w"))
                yield return new WaitForSeconds(0);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentPos, 2.1f);
            buttonHeldDown = false;
        }
        else if (Input.GetKey("s") && buttonHeldDown == false)
        {
            buttonHeldDown = true;
            transform.Translate(new Vector2(0,-2));
            while (Input.GetKey("s"))
                yield return new WaitForSeconds(0);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, currentPos, 2.1f);
            buttonHeldDown = false;
        }
    }
}
