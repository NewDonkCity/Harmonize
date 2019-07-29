using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Rigidbody2D rb;
    public float hSpeed;
    public float vSpeed;

    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity=new Vector2(hSpeed,vSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
