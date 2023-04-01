using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public PlayerControls playerControls;
    private InputAction moveInput;
    private InputAction jump;
    private InputAction dashInput;
    private InputAction dropInput;
    private InputAction crouchInput;
    

    private PlayerStats stats;
    private MovementController playerMovement;

    // "Innate" abilities
    private Jump jumpAbility;
    private Ability dashAbility;

    // Movement
    private float hInput = 0;
    bool jumpBuffered = false;
    private bool dropBuffered;

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
        moveInput = playerControls.Player.Move;
        moveInput.Enable();

        // Register drop before jump for input priority
        dropInput = playerControls.Player.Drop;
        dropInput.Enable();
        dropInput.performed += OnDrop;
        

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += OnJump;

        dashInput = playerControls.Player.Dash;
        dashInput.Enable();
        dashInput.performed += OnDash;

        //crouchInput = playerControls.Player.Crouch;
        //crouchInput.Enable();
        //crouchInput.performed += OnCrouch;
        //crouchInput.canceled += OnCrouchCancel;


    }

    private void OnDisable()
    {
        moveInput.Disable();
        jump.Disable();
        dashInput.Disable();
        dropInput.Disable();
        //crouchInput.Disable();
    }

        private void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<MovementController>();

        jumpAbility = stats.jumpAbility;
        dashAbility = stats.dashAbility;

    }

    public void FixedUpdate()
    {

        // Continuously reads our "Move Input", getting the X value. This will be -1 through 1
        hInput = moveInput.ReadValue<Vector2>().x;
        

    }

    // Drop is a PlayInput with with the . When we double tap the down key we will drop through the platform.
    // 
    public void OnDrop(InputAction.CallbackContext context)
    {
        jumpAbility.Drop();
        dropBuffered = true;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!jumpAbility.isOnCooldown() && jumpAbility.chargesRemaining() > 0)
        {
            UnityEngine.Debug.Log("Jump");
            jumpAbility.beginAbility();
        }
        
    }

    public void OnCrouchCancel(InputAction.CallbackContext context)
    {
        if (dropBuffered)
        {
            dropBuffered = false;
            jumpAbility.StopDropping();
        }
    }



    public void OnDash(InputAction.CallbackContext context)
    {
            dashAbility.beginAbility();        
    }

    public float getHorizontalInput()
    {
        return hInput;
    }

}
