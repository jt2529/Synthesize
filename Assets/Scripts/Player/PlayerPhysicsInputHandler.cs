using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Synthesize.CoreSystem;

[RequireComponent(typeof(PlayerPhysicsInputHandler))]
public class PlayerPhysicsInputHandler : MonoBehaviour
{

    PlayerStats playerStats;
    
    Vector2 input;

    [SerializeField]
   
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    public float hInput;
    public float vInput;

    private Animator animator;
    private AudioSource sound;
    private MovementPhysics physics;

    public float bufferExpireTime;
    public bool jumpBuffered = false;
    public bool jumpReleaseBuffered = false;
    public bool dropBuffered = false;
    public bool hInputBuffered = false;
    public bool dashBuffered = false;
    public Core core { get; private set; }
    private KnockBackReceiver knockBackReceiver { get; set; }


    // Use this for initialization
    void Start()
    {
        core = GetComponentInChildren<Core>();
        physics = core.GetCoreComponent<MovementPhysics>(); //GetComponent<MovementPhysics>();
        playerStats = GetComponent<PlayerStats>();
        GetComponent<PlayerStats>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        knockBackReceiver = core.GetCoreComponent<KnockBackReceiver>();
        input = new Vector2(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        // Here we will check for play input and flag that input has been recieved. We check for these flags
        // in fixedUpdate. Getting input here feels more responsive to the playerStats.

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
        playerStats.aimingDirection = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && vInput < 0) 
        {
            //if (playerStats.objectTouchingBelow != null && (LayerMask.GetMask("OneWayObstacles") & (1 << playerStats.objectTouchingBelow.layer)) != 0)
            //{
                dropBuffered = true;
            //}
            //else 
            //{
                jumpBuffered = true;
                jumpReleaseBuffered = false;
            //}

        }
        else if (value.isPressed)
        {
            jumpBuffered = true;
            jumpReleaseBuffered = false;
            StartCoroutine(JumpBufferExpire(bufferExpireTime));
        }
        else
        {
            jumpReleaseBuffered = true;
            jumpBuffered = false;
            dropBuffered = false;
            StartCoroutine(JumpReleaseBufferExpire(bufferExpireTime));
        }
    }

    public void OnDash(InputValue value) 
    {
        dashBuffered = true;
        StartCoroutine(DashBufferExpire(bufferExpireTime));
    }

    void FixedUpdate()
    {

        
        if (playerStats.GetPlayerAlive() != true)
        {
            animator.SetBool("isAlive", false);
            return;
        }

        hInput = Input.GetAxisRaw("Horizontal");

        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
        }

        input.x = hInput;
        input.y = Input.GetAxisRaw("Vertical");
        
        // hInput is NOT buffered
       /* if (!hInputBuffered)
        {
            playerStats.isWallSliding = false;
        }
        else //hInput is buffered
        {
            if ((playerStats.objectTouchingLeft != null || playerStats.objectTouchingRight != null) && physics.collisions.wallSlope == 90)
            {
                playerStats.isWallSliding = true;
            }
            else 
            {
                playerStats.isWallSliding = false; 
            }
        }*/

        if (physics.ForceX > 0)
        {
            velocity.x = physics.ForceX;
            //physics.ForceX += originalGravity * Time.deltaTime;
            if (physics.ForceX < 0 || physics.collisions.right) { physics.ForceX = 0; }
        }

        if (physics.ForceX < 0)
        {
            velocity.x = physics.ForceX;
            //physics.ForceX -= originalGravity * Time.deltaTime;
            if (physics.ForceX > 0 || physics.collisions.left) { physics.ForceX = 0; }
        }

        if (physics.ForceY > 0)
        {
            velocity.y = physics.ForceY;
            physics.ForceY += playerStats.originalGravity * Time.deltaTime;
            if (physics.ForceY < 0 || physics.collisions.above) { physics.ForceY = 0; }
        }

        if (physics.ForceY < 0)
        {
            velocity.y = physics.ForceY;
            //physics.ForceY -= originalGravity * Time.deltaTime;
            if (physics.ForceY > 0 || physics.collisions.below) { physics.ForceY = 0; }
        }

        if (!physics.canSetVelocity)
        {
            jumpBuffered = false;
            input.x = 0;
        }
        else 
        {
            physics.ForceX = 0;
            physics.ForceY = 0;
        }

        /*if (jumpBuffered && (physics.collisions.below || playerStats.isWallSliding || playerStats.numberOfJumpsLeft > 0))
        {
            if (velocity.y >= playerStats.maxJumpVelocity)
            { 
                velocity.y += playerStats.maxJumpVelocity; 
            }
            else 
            { 
                velocity.y = playerStats.maxJumpVelocity; 
            }
            playerStats.isGrounded = false;
            jumpBuffered = false;
            if (playerStats.isWallSliding && !physics.collisions.below)
            {
                if (playerStats.objectTouchingLeft != null)
                {
                    velocity.x = playerStats.wallJumpPower;
                }
                else 
                {
                    velocity.x = playerStats.wallJumpPower * -1;
                }
                
                velocity.y = playerStats.wallJumpHeight;
            }
            else
            {
                playerStats.numberOfJumpsLeft -= 1;
            }
        }

        if (!physics.collisions.below)
        {
            playerStats.isGrounded = false;
        }
        else 
        {
            playerStats.numberOfJumpsLeft = playerStats.numberOfJumps;
            playerStats.isWallSliding = false;
        }*/

        /*if (jumpReleaseBuffered)
        {
            if (velocity.y > playerStats.minJumpVelocity)
            {
                velocity.y = playerStats.minJumpVelocity;
                jumpReleaseBuffered = false;
            }
        }*/
        /*
        if (playerStats.isDashing)
        {
            velocity.x = playerStats.dashSpeedMultiplier * input.x * playerStats.GetMoveSpeed();
            if (playerStats.isGrounded)
            { 
                velocity.y += playerStats.gravity * Time.deltaTime; 
            }
            else 
            {
                velocity.y = 0;
            }
        }
        else if(playerStats.isDashingEnd) //Need this case to slow player down on dash finish
        {
            velocity.x = input.x * playerStats.GetMoveSpeed();
            velocity.y += playerStats.gravity * Time.deltaTime;
            playerStats.isDashingEnd = false;
        }
        else
        {
            //float targetVelocityX = input.x * playerStats.GetMoveSpeed();
            //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            //velocity.y += playerStats.gravity * Time.deltaTime;
        }

        */

        // Player cannot fall faster than our playerStats.gravity
        if (velocity.y < playerStats.gravity)
        {
            velocity.y = playerStats.gravity;
        }

        if (playerStats.isWallSliding && velocity.y < 0)
        {
            velocity.y += playerStats.wallSlideSpeedDampener * Time.deltaTime;
        }

        //physics.Move(velocity * Time.deltaTime, playerStats);

        if(physics.collisions.below)
        {
            playerStats.isGrounded = true;
        }

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

    IEnumerator DashBufferExpire(float time)
    {
        yield return new WaitForSeconds(time);

        if (dashBuffered == true)
        {
            dashBuffered = false;
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


