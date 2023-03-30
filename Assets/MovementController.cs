using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The new player movement controller

public class MovementController : MonoBehaviour
{

    private PlayerStats stats;
    private MovementPhysics playerPhysics;
    private PlayerInputController playerInput;

    private float hInput = 0;
    private bool jumpBuffered = false;

    // Physics?
    [HideInInspector] public float forceUpward;

    public GameEventScriptableObject playerGroundedEvent;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = GetComponent<MovementPhysics>();
        playerInput = GetComponent<PlayerInputController>();
        stats = GetComponent<PlayerStats>();

        

    }

    private void FixedUpdate()
    {

        hInput = playerInput.getHorizontalInput();

        // Stop falling or rising when a collision is detected
        if (playerPhysics.collisions.above || playerPhysics.collisions.below)
        {
            stats.velocity.y = 0;
        }

        // If we have received a jump input from PlayerInputController, add the Player's jump velocity this frame
        if (jumpBuffered)
        {
            stats.isGrounded = false;
            stats.velocity.y = stats.maxJumpVelocity;
            jumpBuffered = false;
        }





        if (stats.isDashing)
        {
            stats.velocity.x = stats.dashSpeedMultiplier * hInput * stats.moveSpeed;
            if (stats.isGrounded)
            {
                stats.velocity.y += stats.gravity * Time.deltaTime;
            }
            else
            {
                stats.velocity.y = 0;
            }
        }
        else if (stats.isDashingEnd) //Need this case to slow player down on dash finish
        {
            Debug.Log("isDashingEnd");
            stats.velocity.x = hInput * stats.moveSpeed;
            stats.velocity.y += stats.gravity * Time.deltaTime;
            stats.isDashing = false;
            stats.isDashingEnd = false;
        }
        else
        {
            calculateXVelocity();
        }

        if (stats.gravityEnabled)
        {
            CalculateYVelocityWithGravity();
        }

        playerPhysics.Move(stats.velocity * Time.deltaTime);


        if (playerPhysics.collisions.below)
        {
            //stats.isGrounded = true;
            playerGroundedEvent.Raise();
        }
    }

    // jumpBuffered toggles whether we add the Player's jump velocity to the next movement update.
    // This is done in FixedUpdate() above. 
    public void Jump()
    {
        jumpBuffered = true;

    }

    // PlayerInputController passes horizontal input here to determine movement direction.
    // Movement is done in FixedUpdate() above.
    public void HorizontalInput(float input)
    {
        hInput = input;
    }

    // Pulled these out as a function for clarity on what was happening in FixedUpdate()
    //
    private void CalculateYVelocityWithGravity()
    {
        stats.velocity.y += stats.gravity * Time.deltaTime;
        
        // Player cannot fall faster than our gravity
        if (stats.velocity.y < stats.gravity)
        {
            stats.velocity.y = stats.gravity;
        }
    }

    private void calculateXVelocity()
    {
        float targetVelocityX = hInput * stats.moveSpeed;
        stats.velocity.x = Mathf.SmoothDamp(stats.velocity.x, targetVelocityX, ref stats.velocityXSmoothing, (playerPhysics.collisions.below) ? stats.groundAccelerationTime : stats.airAccelerationTime);
    }


}
