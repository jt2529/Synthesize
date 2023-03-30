using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public PlayerControls playerControls;
    private InputAction moveInput;
    private InputAction jump;
    private InputAction dashInput;

    

    private PlayerStats stats;
    private MovementController playerMovement;

    // "Innate" abilities
    private Ability jumpAbility;
    private Ability dashAbility;

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
        moveInput = playerControls.Player.Move;
        moveInput.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += OnJump;

        dashInput = playerControls.Player.Dash;
        dashInput.Enable();
        dashInput.performed += OnDash;
    }

    private void OnDisable()
    {
        moveInput.Disable();
        jump.Disable();
    }

        private void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<MovementController>();

        jumpAbility = stats.jumpAbility;
        

    }

    public void FixedUpdate()
    {

        // Continuously reads our "Move Input", getting the X value. This will be -1 through 1
        hInput = moveInput.ReadValue<Vector2>().x;
        

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!jumpAbility.isOnCooldown() && jumpAbility.chargesRemaining() > 0)
        {
            jumpAbility.beginAbility();
        }
        
    }

    public void OnDash(InputAction.CallbackContext context)
    {
            stats.dashAbility.beginAbility();        
    }

    public float getHorizontalInput()
    {
        return hInput;
    }

}
