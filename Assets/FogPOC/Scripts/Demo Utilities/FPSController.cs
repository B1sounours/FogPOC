using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class FPSController : MonoBehaviour
{
    #region Properties and Variables
    /// <summary>
    /// Ground Move Speed
    /// </summary>
    public float Speed = 10.0f;

    /// <summary>
    /// Air Move Speed
    /// </summary>
    public float AirAccel = 10.0f;

    /// <summary>
    /// Global Gravity
    /// </summary>
    private Vector3 Gravity = Physics.gravity;

    /// <summary>
    /// Player on Ground?
    /// </summary>
    public bool Grounded
    {
        get
        {
            return onGround;
        }
        private set
        {
            onGround = value;

            // Change move state
            if (onGround)
            {
                CurMoveState = MovementState.GROUNDED;
            }
            if (!onGround)
            {
                CurMoveState = MovementState.FLYING;
            }
        }
    }
    private bool onGround = false;

    /// <summary>
    /// Jump Force
    /// </summary>
    private float JumpPower = 5.0f;

    /// <summary>
    /// Multiplier for sphere cast for ground check
    /// </summary>
    const float JumpRayLen = 0.7f;

    /// <summary>
    /// Player Hull Height
    /// </summary>
    private float PlayerHeight;

    /// <summary>
    /// Inputs to nullify
    /// </summary>
    Vector3 nullifiedInput = Vector3.zero;

    // Player-World Collision Hull
    private BoxCollider hull;

    /// <summary>
    /// Indicates movement state
    /// </summary>
    public enum MovementState
    {
        GROUNDED,
        FLYING
    }
    /// <summary>
    /// Current Movement State
    /// </summary>
    public MovementState CurMoveState = MovementState.GROUNDED;

    #endregion

    #region Methods

    /// <summary>
    /// Induce jumping in the player
    /// </summary>
    public void Jump()
    {
        Grounded = false;
        CurMoveState = MovementState.FLYING;
    }

    /// <summary>
    /// Checks if the player is on the ground
    /// </summary>
    /// <returns>True if on ground, otherwise false</returns>
    bool GroundCheck()
    {
        // Create a ray that points down from the centre of the character.
        Ray ray = new Ray(transform.position, -transform.up);

        Vector3 spherePos = transform.position;
        float sphereRad = Mathf.Max(hull.size.x, hull.size.z) / 2.5f;
        float sphereDist = hull.size.y / 2;

        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRad, sphereDist);
        //Debug.Log(sphereRad);
        //Debug.Log(sphereRad * 1.5f);

        if (Grounded || rigidbody.velocity.y < JumpPower * .5f)
        {
            // Default value if nothing is detected:
            Grounded = false;

            // Check every collider hit by the ray
            for (int i = 0; i < hits.Length; i++)
            {
                // Check it's not a trigger
                if (!hits[i].collider.isTrigger &&
                    hits[i].collider != hull)
                {
                    // The character is grounded, and we store the ground angle (calculated from the normal)
                    Grounded = true;

                    /* - under consideration -
                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (rigidbody.velocity.y <= 0)
                    {
                        // also revents you from falling off :(
                        //rigidbody.position = Vector3.MoveTowards(rigidbody.position, hits[i].point + Vector3.up * hull.size.y * .5f, Time.deltaTime * advanced.groundStickyEffect);
                    }
                    */
                    //rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z);
                    break;
                }
            }
        }

        // Draw a ray
        Debug.DrawRay(ray.origin, ray.direction * sphereDist, Grounded ? Color.green : Color.red);

        return Grounded;
    }

    /// <summary>
    /// Performs calculations for the player's velocity as if it were on the ground
    /// </summary>
    /// <returns>New Velocity</returns>
    Vector3 OnGroundMoveCalc(Vector3 playerInput)
    {
        Vector3 TargetVelocity = playerInput;

        TargetVelocity = transform.TransformDirection(TargetVelocity); // Convert direction from local space to world space
        TargetVelocity *= Speed;                                         // Magnify with Player Movespeed

        Vector3 ResultingVelocity = TargetVelocity - rigidbody.velocity;
        ResultingVelocity.y = 0;

        return ResultingVelocity;
    }

    /// <summary>
    /// Performs calculations for the player's velocity as if it were in the air
    /// </summary>
    /// <returns>New Velocity</returns>
    Vector3 MidAirMoveCalc(Vector3 playerInput)
    {
        Vector3 MovementVelocity = playerInput;

        // Nullify Horizontal?
        if (nullifiedInput.x != 0.0f)
        {
            // Clear null if canceled
            if (Mathf.Sign(nullifiedInput.x) * -1 == Mathf.Sign(MovementVelocity.x))
            {
                nullifiedInput = Vector3.zero;
            }

            // Nullify if same sign
            else if (Mathf.Sign(nullifiedInput.x) == Mathf.Sign(MovementVelocity.x))
            {
                MovementVelocity.x = 0.0f;
            }
        }

        // Nullify Vertical?
        if (nullifiedInput.z != 0.0f)
        {
            // Clear null if not applicable or canceled
            if (Mathf.Sign(nullifiedInput.z) * -1 == Mathf.Sign(MovementVelocity.z))
            {
                nullifiedInput = Vector3.zero;
            }

            // Nullify if same sign
            else if (Mathf.Sign(nullifiedInput.z) == Mathf.Sign(MovementVelocity.z))
            {
                MovementVelocity.z = 0.0f;
            }
        }

        MovementVelocity = transform.TransformDirection(MovementVelocity); // Convert direction from local space to world space
        MovementVelocity *= AirAccel;                                      // Magnify with Player Airaccel Speed

        MovementVelocity = new Vector3(Mathf.Clamp(MovementVelocity.x, -AirAccel, AirAccel),
                                       0,
                                       Mathf.Clamp(MovementVelocity.z, -AirAccel, AirAccel)
                                       );

        return MovementVelocity;
    }

    /// <summary>
    /// Retrieves player input as a Vector3
    /// </summary>
    /// <returns>Vector3 where X is "Horizontal" input, where Y is 0, where Z is "Vertical" input</returns>
    Vector3 getInput()
    {
        // Retrieve current input values
        Vector3 MovementVelocity = new Vector3(Input.GetAxis("Horizontal"),  // X    
                                               0,                            // Y
                                               Input.GetAxis("Vertical"));   // Z (think of this as a D-Pad)

        return MovementVelocity;
    }

    /// <summary>
    /// Prevents the player from moving in whatever direction they currently are
    /// </summary>
    void lockInput()
    {
        nullifiedInput = getInput();
    }
    #endregion

    #region Unity Events

    void Start()
    {
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        PlayerHeight = renderer.bounds.extents.y * 2;
        hull = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        // ** Calculate Movement Speed **
        Vector3 AssignedVelocity;

        Vector3 playerInput = getInput();

        switch (CurMoveState)
        {
            case MovementState.GROUNDED:
                {
                    AssignedVelocity = OnGroundMoveCalc(playerInput);

                    rigidbody.AddForce(AssignedVelocity, ForceMode.VelocityChange);

                    onGround = false;
                    break;
                }
            case MovementState.FLYING:
                {
                    AssignedVelocity = MidAirMoveCalc(playerInput);

                    rigidbody.AddForce(AssignedVelocity, ForceMode.Acceleration);

                    break;
                }
            default:
                {
                    break;
                }
        }

        // Apply gravity
        rigidbody.AddForce(Gravity);

        // ** Ground Check **
        GroundCheck();
    }

    void OnCollisionEnter(Collision Other)
    {
        // Lock controls on jump based on last input
        if (Other.gameObject.tag == "Boostpad")
        {
            lockInput();
        }
    }

    void OnTriggerEnter(Collider Other)
    {
        // Lock controls on jump based on last input
        if (Other.tag == "Boostpad")
        {
            lockInput();
        }
    }
    #endregion
}