﻿using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerPhysicsController))]
public class PlayerPhysicsController : MonoBehaviour
{

    PlayerStats stats;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    [SerializeField]
    float gravity;
    float originalGravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    private float hInput;
    private float vInput;
    public float forceUpward;

    private Animator animator;
    private AudioSource sound;
    private MovementPhysics physics;

    private bool jumpBuffered = false;
    private bool jumpReleaseBuffered = false;
    private bool hInputBuffered = false;

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

    public void OnMove(InputValue value)
    {
        hInput = value.Get<Vector2>().x;
        vInput = value.Get<Vector2>().y;
    }

    public void OnLook(InputValue value)
    {
        stats.aimingDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && vInput < 0) 
        {
            if (stats.objectTouchingBelow != null && (LayerMask.GetMask("OneWayObstacles") & (1 << stats.objectTouchingBelow.layer)) != 0)
            {
                stats.isDropping = true;
            }
            else 
            {
                jumpBuffered = true;
                jumpReleaseBuffered = false;
            }
            
        }
        else if (value.isPressed)
        {
            jumpBuffered = true;
            jumpReleaseBuffered = false;
            //StartCoroutine(JumpBufferExpire(0.10f));
        }
        else
        {
            jumpReleaseBuffered = true;
            jumpBuffered = false;
            stats.isDropping = false;
            //StartCoroutine(JumpReleaseBufferExpire(0.10f));
        }
    }

    public void OnDash(InputValue value) 
    {
        if (stats.numberOfDashesLeft > 0)
        {
            stats.numberOfDashesLeft -= 1;
            stats.isDashing = true;
            stats.dashTimeLeft = stats.fullDashTime;
        }
        
    }

    void FixedUpdate()
    {

        
        if (stats.GetPlayerAlive() != true)
        {
            animator.SetBool("isAlive", false);
            return;
        }

        

        updatePlayerPhysics();

        hInput = Input.GetAxisRaw("Horizontal");

        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
            if (physics.collisions.above) 
            {
                forceUpward = 0;
            }
        }

        Vector2 input;
        input.x = hInput;
        input.y = Input.GetAxisRaw("Vertical");
        
        // hInput is NOT buffered
        if (!hInputBuffered)
        {
            stats.isWallSliding = false;
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

            if ((stats.objectTouchingLeft != null || stats.objectTouchingRight != null) && physics.collisions.wallSlope == 90)
            {
                stats.isWallSliding = true;
            }
            else 
            { 
                stats.isWallSliding = false; 
            }
        }



        if (jumpBuffered && (physics.collisions.below || stats.isWallSliding || stats.numberOfJumpsLeft > 0))
        {
            velocity.y = maxJumpVelocity;
            stats.isGrounded = false;
            jumpBuffered = false;
            if (stats.isWallSliding && !physics.collisions.below)
            {
                if (stats.objectTouchingLeft != null)
                {
                    velocity.x = stats.wallJumpPower;
                }
                else 
                {
                    velocity.x = stats.wallJumpPower * -1;
                }
                
                velocity.y = stats.wallJumpHeight;
            }
            else
            {
                stats.numberOfJumpsLeft -= 1;
            }
        }

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

        if (!physics.collisions.below)
        {
            stats.isGrounded = false;
        }
        else 
        {
            stats.numberOfJumpsLeft = stats.numberOfJumps;
            stats.isWallSliding = false;
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
            velocity.x = stats.dashSpeedMultiplier * input.x * stats.GetMoveSpeed();
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
            velocity.x = input.x * stats.GetMoveSpeed();
            velocity.y += gravity * Time.deltaTime;
            stats.isDashingEnd = false;
        }
        else
        {
            float targetVelocityX = input.x * stats.GetMoveSpeed();
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
        }
        
                    
        // Player cannot fall faster than our gravity
        if (velocity.y < gravity)
        {
            velocity.y = gravity;
        }

        if (stats.isWallSliding && velocity.y < 0)
        {
            velocity.y += stats.wallSlideSpeedDampener * Time.deltaTime;
        }

        physics.Move(velocity * Time.deltaTime, stats);

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
        animator.SetBool("isWallSliding", stats.isWallSliding);
    }

    void updatePlayerPhysics()
    {
        gravity = -(2 * stats.GetMaxJumpHeight()) / Mathf.Pow(stats.GetTimeToJumpApex(), 2);
        maxJumpVelocity = Mathf.Abs(gravity) * stats.GetTimeToJumpApex();
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.GetMinJumpHeight());
    }

    IEnumerator JumpBufferExpire(float time)
    {
        yield return new WaitForSeconds(time);

        if(jumpBuffered == true)
        {
            jumpBuffered = false;
        }
    }

    IEnumerator JumpReleaseBufferExpire(float time)
    {
        yield return new WaitForSeconds(time);

        if (jumpReleaseBuffered == true)
        {
            jumpReleaseBuffered = false;
        }
    }

    public void OnMove()
    {
        
    }

    public bool getJumpBuffered()
    {
        return jumpBuffered;
    }
}


