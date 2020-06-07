using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Jumpable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Liftable))]
[RequireComponent(typeof(SpriteRenderer))]

/*
    This script is used to control the player. Current controls include:
    - Walking
    - Running
    - Jumping
    - Lifting/Throwing/Dropping
*/
public class PlayerMovement : MonoBehaviour
{
    //======================================================
    // Notes
    //======================================================
    /*
        Paul Nov 15, 2019:
        GetAxis returns a value anywhere from -1 to 1 depending on the input button
        being pressed. For example:
            - upKey pressed: -> 1
            - downKey pressed -> -1
            - no key presseed -> 0
        These values are used to control the magnifude/direction of character speed.

        Movement in unity can be done using RigidBody, Translation, or 
        CharacterController. Translation/character controller are better used for 
        controlling objects not impacted by physics.
        Since physics are being used, RigidBody will be used for player movement.
        Rigidbody vs Translation: https://www.youtube.com/watch?v=ixM2W2tPn6c
        Rigidbody vs CharacterController: https://www.youtube.com/watch?v=AEPI5rmg3XY

        Multiply movement speeds by Time.deltaTime to ensure that they are not frame
        rate dependent. I.e. they will run at the same speed regardless of computer
        performance.

        Paul Dec 21, 2019:
        We might want to use a box collider for the player rather than a copsule
        collider. A box collider has a flat top and will allow players to stack easier.
    */

    //======================================================
    // Instance variables
    //======================================================
    // [Header("Movement")]
    /// <summary>
    /// The speed at which the player moves. This value is toggled
    /// between _walkSpeed and _runSpeed depending on user input.
    /// </summary>
    [SerializeField] private float _moveSpeed = 0.0f;
    /// <summary>
    /// The walk speed of the player.
    /// </summary>
    [SerializeField] private float _walkSpeed = 2.0f;
    /// <summary>
    /// The run speed of the player.
    /// </summary>
    [SerializeField] private float _runSpeed = 4.0f;
    /// <summary>
    /// The magnitude of the impulse used to jump the player.
    /// </summary>
    [SerializeField] private float _jumpSpeed = 3.0f;

    //======================================================   
    // [Header("Lifting")]
    /// <summary>
    /// The range at which players may grab objects.
    /// Range is from the centroid of the player to the face of the object.
    /// </summary>
    [SerializeField] private float _liftRange = 0.15f;
    /// <summary>
    /// The height above the player centroid that the object origin is lifted.
    /// This is set to the lift height of the specific object as it is being lifted.
    /// </summary>
    [SerializeField] private float _liftHeight = 0.23f;
    /// <summary>
    /// The magnitude of the impulse used to throw objects horizontally.
    /// </summary>
    [SerializeField] private float _throwSpeed = 1.0f;
    /// <summary>
    /// The magnitude of the impulse used to throw objects vertically.
    /// </summary>
    [SerializeField] private float _verticalThrowLift = 3.0f;
    /// <summary>
    /// The GameObject that the player is carrying (if any).
    /// </summary>
    private GameObject _lifted = null;

    //====================================================== 
    // [Header("Indicators")]
    /// <summary>
    /// The magnitude of the impulse used to throw objects vertically.
    /// </summary>
    private bool _isGrounded = true;

    //====================================================== 
    // [Header("Components")] 
    /// <summary>
    /// The Animator used to toggle between player sprite animations.
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// The BoxCollider used to detect player collisions.
    /// </summary>
    private BoxCollider _boxCollider;
    /// <summary>
    /// The Liftable used to indicate that the player can be lifted.
    /// </summary>
    private Liftable _liftable;
    /// <summary>
    /// The Ridigbody used to move and induce physics on the player.
    /// </summary>
    private Rigidbody _rigidBody;
    /// <summary>
    /// The SpriteRenderer used to render the player.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    //======================================================    
    // [Header("Vectors")]
    /// <summary>
    /// The Liftable used to indicate that the player can be lifted.
    /// </summary>
    private Vector3 _movementDirection;
    /// <summary>
    /// The Liftable used to indicate that the player can be lifted.
    /// </summary>
    private Vector3 _facingDirection;

    //======================================================
    // MonoBehaviour
    //======================================================
    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    private void Update()
    {
        CheckInputs();
        UpdateAnimatorVariables();
        Lifting(); // Used in Update rather than FixedUpdate for no lag
    }

    /// <summary>
    /// Called every physics update.
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer(_movementDirection);
    }

    //======================================================
    // Helper methods
    //======================================================
    /// <summary>
    /// Initializes the player movement instance variables.
    /// </summary>
    private void Init()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
        _liftable = GetComponent<Liftable>();
        _rigidBody = GetComponent<Rigidbody>();
        _spriteRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
    }

    private void CheckInputs()
    {
        CheckWalkInput();
        CheckJumpInput();
        CheckRunInput();
        CheckLiftInput();
        CheckDropInput();
    }

    /// <summary>
    /// Checks for walk inputs and stores them as a 3D vector to determine 
    /// player movement direction. A normalized version of latest non-zero
    /// movement direction is stored to indicate the direction that the player
    /// if facing. Also horizontally flips sprite when player moves left or right.
    /// </summary>
    private void CheckWalkInput()
    {
        // Get player inputs and store them
        float leftAndRight = Input.GetAxis("Horizontal");
        float forwardAndBack = Input.GetAxis("Vertical");
        _movementDirection = new Vector3(leftAndRight, 0, forwardAndBack);

        // Store last non-zero "facing" direction for raycasting purposes
        if (_movementDirection != new Vector3(0, 0, 0))
        {
            // Normalize vector so throwing distance not dependent on
            // magnitude of movement input.
            _facingDirection = _movementDirection.normalized;
        }

        // Flip sprite if left/right movement made
        if (leftAndRight < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (leftAndRight > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// Jumps the player if jump input is detected.
    /// </summary>
    private void CheckJumpInput()
    {
        // If player presses space while on ground...
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            JumpPlayer();
        }
    }

    /// <summary>
    /// Changes _moveSpeed to _runSpeed if run input detected.
    /// Changes _moveSpeed to _walkSpeed if run input not detected.
    /// </summary>
    private void CheckRunInput()
    {
        // If player holds shift key...
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveSpeed = _runSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }
    }

    /// <summary>
    /// Attempts to lift object if player not already holding anything.
    /// Otherwise, attempts to drop object that player is holding.
    /// </summary>
    private void CheckLiftInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_lifted == null)
            {
                Lift();
            }
            else
            {
                Throw();
            }
        }
    }

    /// <summary>
    /// Attempts to drop object if the player is currently holding something.
    /// </summary>
    private void CheckDropInput()
    {
        // If R is pressed and player is holding an object...
        if (Input.GetKeyDown(KeyCode.R) && _lifted != null)
        {
            Drop();
        }
    }

    /// <summary>
    /// Assigns object to _lifted if it is in range and has Liftable.cs.
    /// </summary>
    private void Lift()
    {
        // Send ray in front of player
        Ray ray = new Ray(transform.position, _facingDirection);
        RaycastHit hitInfo;
        // If the ray hits something within range that contains a Liftable.cs component...
        if (Physics.Raycast(ray, out hitInfo, _liftRange) && isLiftable(hitInfo))
        {
            // Draw red line for editor view (for debug purposes)
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            // Set _lifted to lifted object
            _lifted = hitInfo.transform.gameObject;
            // Toggle _isLifted in Liftable.cs for lifted object
            Liftable liftable = _lifted.GetComponent<Liftable>();
            liftable.set_IsLifted(true);
            // Receive the _liftHeight of the object
            _liftHeight = liftable.get_liftHeight();
        }
        else
        {
            // Draw green line for editor view (for debug purposes)
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }

    /// <summary>
    /// If player is holding an object (AKA _lifted != null), 
    /// holds the object above the player by _liftHeight.
    /// </summary>
    private void Lifting()
    {
        // If the player is carrying something...
        if (_lifted != null)
        {
            // Set _lifted tansform slightly above player transform
            _lifted.transform.position = (transform.position + new Vector3(0f, _liftHeight, 0f));
        }
    }

    /// <summary>
    /// Checks whether an object detected by raycasting holds Liftable.cs
    /// </summary>
    /// <param name="hitInfo"> 
    /// RaycastHit containing information about the object it touched.
    /// </param> 
    /// <returns>
    /// True if detected object holds Liftable.cs, false otherwise.
    /// </returns>
    private bool isLiftable(RaycastHit hitInfo)
    {
        if (hitInfo.collider.gameObject.GetComponent<Liftable>() != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Throws the object that the player is currently lifting.
    /// </summary>
    private void Throw()
    {
        // Set _isLifted to false in Liftable.cs
        _lifted.GetComponent<Liftable>().set_IsLifted(false);
        // Reset object velocity to zero
        Rigidbody rb = _lifted.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // Throw the object up and in the direction player is facing 
        Vector3 throwDirection = _facingDirection + new Vector3(0,_verticalThrowLift,0);
        rb.AddForce(throwDirection * _throwSpeed, ForceMode.Impulse);
        // Set _lifted to null to indicate player not holding anything
        _lifted = null;
    }

    /// <summary>
    /// Drops the object that the player is currently lifting.
    /// </summary>
    private void Drop()
    {
        // Set _isLifted to false in Liftable.cs
        _lifted.GetComponent<Liftable>().set_IsLifted(false);
        // Reset object velocity to zero 
        Rigidbody rb = _lifted.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        // Set _lifted to null to indicate player not holding anything
        _lifted = null;
    }

    /// <summary>
    /// Updates variables necessary for the animator to determine appropriate 
    /// animations.
    /// </summary>
    private void UpdateAnimatorVariables()
    {
        // Update animator variables to reflect player movement
        _animator.SetFloat("speed", _movementDirection.magnitude);
        _animator.SetBool("isGrounded", _isGrounded);
    }

    /// <summary>
    /// Moves player in input direction if they are not lifted.
    /// </summary>
    /// <param name="direction"> 
    /// Vector direction/magnitude of player movement.
    /// </param> 
    private void MovePlayer(Vector3 direction)
    {
        // If player is not lifted, move player
        if (_liftable.get_isLifted() == false)
        {
            _rigidBody.MovePosition(transform.position + (direction * _moveSpeed * Time.deltaTime));
        }
    }

    /// <summary>
    /// Verically jumps player.
    /// </summary>
    private void JumpPlayer()
    {
        // Add vertical force
        _rigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        // Toggle indicators
        _isGrounded = false;
    }

    /// <summary>
    /// Sets isGrounded to true when player collides with an object that
    /// is holding the "Jumpable" script.
    /// </summary>
    /// <param name="other"> 
    /// Collision associated with GameObject touching the player.
    /// </param> 
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Jumpable>() != null)
        {
            _isGrounded = true;
        }
    }

}
