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
    private bool wallJumpBuffered = false;
    private Vector2 externalForce;
    // Physics?
    [HideInInspector] public float forceUpward;

    [Header("Events")]
    public GameEventScriptableObject playerJumpEvent;
    public GameEventScriptableObject playerWallJumpEvent;
    public GameEventScriptableObject playerGroundedEvent;
    public GameEventScriptableObject playerRisingEvent;
    public GameEventScriptableObject playerFallingEvent;


    // Start is called before the first frame update
    void Start()
    {

        playerPhysics = GetComponent<MovementPhysics>();
        playerInput = GetComponent<PlayerInputController>();
        stats = GetComponent<PlayerStats>();

        externalForce = Vector2.zero;

    }

    private void FixedUpdate()
    {

        hInput = playerInput.getHorizontalInput();

        // Stop falling or rising when a collision is detected
        if (playerPhysics.collisions.above || playerPhysics.collisions.below)
        {
            stats.velocity.y = 0;
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

        // If we have received a jump input from PlayerInputController, add the Player's jump velocity this frame
        if (jumpBuffered)
        {
            playerJumpEvent.Raise(); // Let anything that cares know we jumped
            stats.velocity.y = stats.maxJumpVelocity;
            jumpBuffered = false;
        }
        else if(wallJumpBuffered)
        {
            playerWallJumpEvent.Raise();
            stats.velocity.y = stats.maxJumpVelocity;
            stats.velocity.x = stats.wallJumpHorizontalForce * -stats.PlayerFacingDirection();
            wallJumpBuffered = false;
            
        }



        if (stats.gravityEnabled)
        {
            CalculateYVelocityWithGravity();

            if (stats.IsWallSliding() && !stats.IsWallJumping())
            {
                stats.velocity.y = stats.velocity.y * stats.wallSlideSpeedDampener;
            }
        }

        if(stats.velocity.y > 0)
        {
            playerRisingEvent.Raise();
            RemoveCollisionWithOneWayObstacles();
        }
        else if(stats.velocity.y < 0)
        {
            playerFallingEvent.Raise();
            if (!stats.IsDropping())
            {
                ResumeCollisionWithOneWayObstacles();
            }
            
        }
        
        playerPhysics.Move((stats.velocity + externalForce) * Time.deltaTime);
        
        

        if (playerPhysics.collisions.below)
        {
            playerGroundedEvent.Raise();
            ResumeCollisionWithOneWayObstacles();
        }
    }

    // jumpBuffered toggles whether we add the Player's jump velocity to the next movement update.
    // This is done in FixedUpdate() above. 
    public void Jump()
    {
        jumpBuffered = true;

    }

    public void WallJump(float xForce)
    {
        wallJumpBuffered = true;
        //externalForce.x = xForce * -stats.PlayerFacingDirection(); //apply a force on the X axis that is opposite of the direction of the surface we are wall sliding on.
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

    public void RemoveCollisionWithOneWayObstacles()
    {
        playerPhysics.ApplyNewCollisionMask(LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles"));
    }

    public void ResumeCollisionWithOneWayObstacles()
    {
        playerPhysics.ApplyNewCollisionMask(LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles", "OneWayObstacles"));
    }

    public bool CollisionLeft()
    {
        return playerPhysics.collisions.left;
    }

    public bool CollisionRight()
    {
        return playerPhysics.collisions.right;
    }

    public void applyExternalForce(Vector2 force)
    {
        externalForce = force;
    }

    
}
