using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public PlayerControls playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction dash;

    public Ability firstAbility;
    public Ability secondAbility;
    public Ability thirdAbility;   
    public Ability fourthAbility;

    private PlayerStats stats;
    private MovementController playerMovement;
    //private MovementPhysics playerPhysics;

    // Movement
    public float hInput = 0;
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

        dash = playerControls.Player.Dash;
        dash.Enable();
        dash.performed += OnDash;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

        private void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<MovementController>();
        //playerPhysics = GetComponent<MovementPhysics>();

        updatePlayerPhysics();
    }

    public void FixedUpdate()
    {

        // Reads our "Move Input", getting the X value. This will be -1 through 1
        hInput = move.ReadValue<Vector2>().x;
        playerMovement.HorizontalInput(hInput);

    }

    void updatePlayerPhysics()
    {
        stats.gravity = -(2 * stats.maxJumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        stats.maxJumpVelocity = Mathf.Abs(stats.gravity) * stats.timeToJumpApex;
        stats.minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(stats.gravity) * stats.minJumpHeight);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (stats.JumpAllowed())
        {
            playerMovement.BufferJumpInput();
        }
        
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash Pressed");
    }



}
