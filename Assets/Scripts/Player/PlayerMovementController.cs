using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;


public class PlayerMovementController : MonoBehaviour
{

    public PlayerControls playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    PlayerStats stats;

    float gravity;
    float originalGravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    private float hInput;
    [HideInInspector] public float forceUpward;

    private Animator animator;
    private AudioSource sound;
    MovementPhysics physics;

    private bool jumpBuffered = false;
    private bool jumpReleaseBuffered = false;
    private bool hInputBuffered = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += OnJump;

        dash = playerControls.Player.Jump;
        dash.Enable();
        dash.performed += OnDash;

    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
    }

        // Use this for initialization
        void Start()
    {
        physics = GetComponent<MovementPhysics>();
        stats = GetComponent<PlayerStats>();
        updatePlayerPhysics();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        originalGravity = gravity;
    }
    // Update is called once per frame
    void Update()
    {
        // Here we will check for play input and flag that input has been recieved. We check for these flags
        // in fixedUpdate. Getting input here feels more responsive to the player.

        if (hInput != 0)
        {
            hInputBuffered = true;
        } else
        {
            hInputBuffered = false;
        }
    }

    public void OnLook(InputValue value)
    {
        stats.aimingDirection = value.Get<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            jumpBuffered = true;
            jumpReleaseBuffered = false;
            //StartCoroutine(JumpBufferExpire(0.10f));
        }
        else 
        {
            jumpReleaseBuffered = true;
            jumpBuffered = false;
            stats.numberOfJumpsLeft -= 1;
            //StartCoroutine(JumpReleaseBufferExpire(0.10f));
        }
    }

    public void OnDash(InputAction.CallbackContext context) 
    {
        if (stats.numberOfDashesLeft > 0)
        {
            stats.numberOfDashesLeft -= 1;
            stats.isDashing = true;
            stats.dashTimeRemaining = stats.fullDashTime;
        }
        
    }

    void FixedUpdate()
    {

        
        
        
        if (!stats.playerAlive)
        {
            animator.SetBool("isAlive", false);
            return;
        }

        

        updatePlayerPhysics();
        hInput = move.ReadValue<Vector2>().x;
        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
            if (physics.collisions.above) 
            {
                forceUpward = 0;
            }
        }

        //Vector2 input = new Vector2(move.ReadValue<Vector2>().x, jump.ReadValue<float>());
        
        // This block determines which direction the player is facing based on the hInput. 
        // This seems very buggy and needs changed.
        if (!hInputBuffered)
        {
            
            stats.isRunning = false;
        }
        else //hInput is buffered
        {
            stats.isRunning = true;
            if (hInput > 0)
            {
                if (!stats.facingRight) 
                {
                    transform.localScale = new Vector3(1,1,1);
                }
                //sprite.flipX = false;
                stats.facingRight = true;
            }
            if (hInput < 0)
            {
                if (stats.facingRight)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                //sprite.flipX = true;
                stats.facingRight = false;
            }
        }


        // Disables our jump input if we have no more jumps left.
        // Enabled/Disabled should be handdled at the player stats and input level I think and will need to be moved.
        if(jumpBuffered && stats.numberOfJumpsLeft <= 0)
        {
            jumpBuffered = false;
        }

        if (jumpBuffered && (physics.collisions.below || stats.numberOfJumpsLeft > 0))
        {
            velocity.y = maxJumpVelocity;
            stats.isGrounded = false;
            jumpBuffered = false;
            stats.numberOfJumpsLeft -= 1;
        }

        // Applies an upwards force on the player, such as a bounce pad.
        // Currently only applies this force directly upwards.
        if (forceUpward > 0) 
        {
            stats.isGrounded = false;
            velocity.y = forceUpward;
            forceUpward += originalGravity * Time.deltaTime;
            if (forceUpward < 0)
            {
                forceUpward = 0;
            }
        }

        // Not sure what is happening here.
        if (!physics.collisions.below) // No collisions below
        {
            if (stats.isGrounded) // So if we have no collisions below, but our player is considered on the ground
            {
                stats.numberOfJumpsLeft -= 1; // reduce the number of jumps we have remaining?
            }
            stats.isGrounded = false; // then set the player state to grounded.
        }
        else 
        {
            stats.numberOfJumpsLeft = stats.maxNumberofJumps; // If we have a collision below we get all of our jumps back
        }

        if (jumpReleaseBuffered)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
                jumpReleaseBuffered = false;
            }
        }

        if (stats.isDashing)
        {
            velocity.x = stats.dashSpeedMultiplier * move.ReadValue<Vector2>().x * stats.moveSpeed;
            if (stats.isGrounded)
            { 
                velocity.y += gravity * Time.deltaTime; 
            }
            else 
            {
                velocity.y = 0;
            }
        }
        else if(stats.isDashingEnd) //Need this case to slow player down on dash finish
        {
            velocity.x = move.ReadValue<Vector2>().x * stats.moveSpeed;
            velocity.y += gravity * Time.deltaTime;
            stats.isDashingEnd = false;
        }
        else
        {
            float targetVelocityX = move.ReadValue<Vector2>().x * stats.moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? stats.groundAccelerationTime : stats.airAccelerationTime);
            velocity.y += gravity * Time.deltaTime;
        }
        

        // Player cannot fall faster than our gravity
        if (velocity.y < gravity)
        {
            velocity.y = gravity;
        }

        physics.Move(velocity * Time.deltaTime);

        if(physics.collisions.below)
        {
            stats.isGrounded = true;
            
        }

        updateAnimationState();
    }

    void updateAnimationState()
    {
        animator.SetBool("isRunning", stats.isRunning);
        animator.SetBool("isGrounded", stats.isGrounded);
        animator.SetBool("isAlive", stats.isPlayerAlive());
        animator.SetBool("isDashing", stats.isDashing);
    }

    void updatePlayerPhysics()
    {
        gravity = -(2 * stats.maxJumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * stats.timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.minJumpHeight);
    }

    public bool getJumpBuffered()
    {
        return jumpBuffered;
    }
}


