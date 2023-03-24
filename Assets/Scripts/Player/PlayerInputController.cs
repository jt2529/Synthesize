using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public PlayerControls playerControls;
    public PlayerMovementController playerMovementController;


    private InputAction move;
    private InputAction jump;
    private InputAction dash;
    private InputAction firePrimary;
    private InputAction fireSecondary;

    private Vector2 moveDirection;
    private bool jumpPressed = false;

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
        
        dash = playerControls.Player.Dash;
        dash.Enable();

        firePrimary = playerControls.Player.FirePrimary;
        firePrimary.Enable();
        firePrimary.performed += FirePrimary;

        fireSecondary = playerControls.Player.FireSecondary;
        fireSecondary.Enable();
        fireSecondary.performed += FireSecondary;

    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
        firePrimary.Disable();
        fireSecondary.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
     
    }

    private void FirePrimary(InputAction.CallbackContext context)
    {
        Debug.Log("Primary Ability Fired");
    }

    private void FireSecondary(InputAction.CallbackContext context)
    {
        Debug.Log("Secondary Ability Fired");
    }

}
