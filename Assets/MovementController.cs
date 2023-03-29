using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    private PlayerStats stats;
    private MovementPhysics playerPhysics;

    private float hInput = 0;
    private bool jumpBuffered = false;

    // Physics?
    [HideInInspector] public float forceUpward;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = GetComponent<MovementPhysics>();
        stats = GetComponent<PlayerStats>();

    }

    private void FixedUpdate()
    {
        // Stop falling or rising when a collision is detected
        if (playerPhysics.collisions.above || playerPhysics.collisions.below)
        {
            stats.velocity.y = 0;
        }

        if (jumpBuffered)
        {
            Jump();
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
            stats.isGrounded = true;
            stats.ResetJumps();

        }
    }

    public void BufferJumpInput()
    {
        jumpBuffered = true;
    }

    public void Jump()
    {
        stats.UseJump();
        stats.velocity.y = stats.maxJumpVelocity;
        jumpBuffered = false;
    }

    public void HorizontalInput(float input)
    {
        hInput = input;
    }

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
