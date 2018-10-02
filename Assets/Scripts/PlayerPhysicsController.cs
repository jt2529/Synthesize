﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysicsController))]
public class PlayerPhysicsController : MonoBehaviour
{

    PlayerStats stats;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    private float hInput;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isRunning;
    private Animator animator;

    MovementPhysics physics;

    // Use this for initialization
    void Start()
    {
        physics = GetComponent<MovementPhysics>();
        stats = GetComponent<PlayerStats>();
        updatePlayerPhysics();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        
        if (stats.GetPlayerAlive() != true)
        {
            return;
        }

        updatePlayerPhysics();

        hInput = Input.GetAxisRaw("Horizontal");

        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (hInput == 0)
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

            isRunning = false;
            
        }
        else
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

        if (Input.GetButton("Jump") && physics.collisions.below)
        {
            velocity.y = maxJumpVelocity;
            isGrounded = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        float targetVelocityX = input.x * stats.GetMoveSpeed();
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
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
    }

    void updatePlayerPhysics()
    {
        gravity = -(2 * stats.GetMaxJumpHeight()) / Mathf.Pow(stats.GetTimeToJumpApex(), 2);
        maxJumpVelocity = Mathf.Abs(gravity) * stats.GetTimeToJumpApex();
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.GetMinJumpHeight());
    }
}
