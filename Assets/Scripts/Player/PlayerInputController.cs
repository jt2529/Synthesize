using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;

    public Ability firstAbility;
    public Ability secondAbility;
    public Ability thirdAbility;   
    public Ability fourthAbility;

    private PlayerStats stats;
    private MovementPhysics playerPhysics;

    // Movement
    private float hInput = 0;
    bool jumpBuffered = false;

    // Physics?
    [HideInInspector] public float forceUpward;

    // Events
    [Header("Events")]
    public GameEventScriptableObject playerJumped;
    [Space(5)]
    public GameEventScriptableObject playerAirborne;
    public GameEventScriptableObject playerGrounded;


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
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

        private void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerPhysics = GetComponent<MovementPhysics>();
    }

    public void FixedUpdate()
    {

        hInput = move.ReadValue<Vector2>().x;

        updatePlayerPhysics();
        // Stop falling or rising when a collision is detected
        if (playerPhysics.collisions.above || playerPhysics.collisions.below)
        {
            stats.velocity.y = 0;
        }

        if (jumpBuffered && stats.numberOfJumpsLeft <= 0)
        {
            jumpBuffered = false;
        }

        if (jumpBuffered && (playerPhysics.collisions.below || stats.numberOfJumpsLeft > 0))
        {
            stats.velocity.y = stats.maxJumpVelocity;
            
            playerAirborne.Raise();
            playerJumped.Raise();

            jumpBuffered = false;
        }

        float targetVelocityX = hInput * stats.moveSpeed;
        stats.velocity.x = Mathf.SmoothDamp(stats.velocity.x, targetVelocityX, ref stats.velocityXSmoothing, (playerPhysics.collisions.below) ? stats.groundAccelerationTime : stats.airAccelerationTime);
        stats.velocity.y += stats.gravity * Time.deltaTime;

        // Player cannot fall faster than our gravity
        if (stats.velocity.y < stats.gravity)
        {
            stats.velocity.y = stats.gravity;
        }

        playerPhysics.Move(stats.velocity * Time.deltaTime);
        if (playerPhysics.collisions.below)
        {
            stats.isGrounded = true;

        }
    }

    void updatePlayerPhysics()
    {
        stats.gravity = -(2 * stats.maxJumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        stats.maxJumpVelocity = Mathf.Abs(stats.gravity) * stats.timeToJumpApex;
        stats.minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(stats.gravity) * stats.minJumpHeight);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
        jumpBuffered = true;
        
    }



}
