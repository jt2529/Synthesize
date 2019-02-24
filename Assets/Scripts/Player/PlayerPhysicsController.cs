using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysicsController))]
public class PlayerPhysicsController : MonoBehaviour
{

    PlayerStats stats;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    [SerializeField]
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    private float hInput;
    private float vInput;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isRunning;
    private Animator animator;
    private AudioSource sound;
    MovementPhysics physics;

    private bool jumpBuffered = false;
    private bool jumpReleaseBuffered = false;
    private bool hInputBuffered = false;
    private bool vInputBuffered = false;

    // Use this for initialization
    void Start()
    {
        physics = GetComponent<MovementPhysics>();
        stats = GetComponent<PlayerStats>();
        updatePlayerPhysics();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        // Here we will check for play input and flag that input has been recieved. We check for these flags
        // in fixedUpdate. Getting input here feels more responsive to the player.
        if (Input.GetButtonDown("Jump"))
        {

            jumpBuffered = true;
            StartCoroutine(JumpBufferExpire(0.10f));
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpReleaseBuffered = true;
            StartCoroutine(JumpReleaseBufferExpire(0.10f));
        }

        hInput = Input.GetAxisRaw("Horizontal");

        if(hInput != 0)
        {
            hInputBuffered = true;
        } else
        {
            hInputBuffered = false;
        }

       vInput = Input.GetAxisRaw("Horizontal");

        if (vInput != 0)
        {
            vInputBuffered = true;
        }
        else
        {
            vInputBuffered = false;
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
        }

        Vector2 input = new Vector2(hInput, Input.GetAxisRaw("Vertical"));
        
        // hInput is NOT buffered
        if (!hInputBuffered)
        {
            if (vInput != 0)
            {
                stats.aimingDirection.x = 0;
                stats.aimingDirection.y = input.y;
            }
            else
            {
                stats.aimingDirection.y = 0;
                if (sprite.flipX == true)
                {
                    stats.aimingDirection.x = -1;
                }
                else
                {
                    stats.aimingDirection.x = 1;
                }
            }
            isRunning = false;
        }
        else //hInput is buffered
        {
            isRunning = true;

            stats.aimingDirection.x = input.x;
            stats.aimingDirection.y = input.y / 2;
            if (hInput > 0)
            {
                sprite.flipX = false;
            }
            if (hInput < 0)
            {
                sprite.flipX = true;
            }
        }

        if (jumpBuffered && physics.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            sound.Play();
            isGrounded = false;
            jumpBuffered = false;
        }

        if (jumpReleaseBuffered)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
                jumpReleaseBuffered = false;
            }
        }

        float targetVelocityX = input.x * stats.GetMoveSpeed();
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        // Player cannot fall faster than our gravity
        if (velocity.y < gravity)
        {
            velocity.y = gravity;
        }

        physics.Move(velocity * Time.deltaTime);

        if(physics.collisions.below)
        {
            isGrounded = true;
        }

        updateAnimationState();
    }

    void updateAnimationState()
    {
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isAlive", stats.isPlayerAlive());   
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
}
