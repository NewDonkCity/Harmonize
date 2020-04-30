using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/*
    This script indicates whether an object
    can be lifted by the player. It must be included on all
    objects that the player is expected to lift.
*/
public class Liftable : MonoBehaviour
{
    //======================================================
    // Instance variables
    //======================================================
    /// <summary>
    /// The Rigidbody used to constrain object rotation.
    /// </summary>
    private Rigidbody _rigidbody;
    /// <summary>
    /// Boolean that indicates whether an object is currently being lifted.
    /// </summary>
    private bool _isLifted = false;
    /// <summary>
    /// The height above the player centroid that the object origin is lifted.
    /// </summary>
    private float _liftHeight = 0.23f;

    //======================================================
    // MonoBehaviour
    //======================================================
    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {

    }

    //======================================================
    // Helper methods
    //======================================================
    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //======================================================
    // Getters and setters
    //======================================================
    public bool get_isLifted()
    {
        return _isLifted;
    }

    public float get_liftHeight()
    {
        return _liftHeight;
    }
    
    /// <summary>
    /// Toggles the _isLifted instance variable.
    /// If the object is lifted, rotation is reset and locked.
    /// If the object is let go, rotation is unlocked.
    /// </summary>
    /// <param name="b"> 
    /// Desired setting value for _isLifted.
    /// </param> 
    public void set_IsLifted(bool b)
    {
        if (b == true)
        {
            _isLifted = true;
            transform.rotation = Quaternion.identity; // Resets rotation
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _isLifted = false;
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

}