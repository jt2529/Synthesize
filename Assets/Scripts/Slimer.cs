using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimer : MonoBehaviour {

    private MovementPhysics controller;
    private bool right;
    float velocityXSmoothing;
    private float gravity;
    public EnemyStats stats;
    private float movementDampener;
    private BeatTimer beatTimer;

    [SerializeField]
    private float forceThreshold = .01f;

    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    private float direction = 1.0f;
    private SpriteRenderer sprite;

    Vector3 velocity;

    // Use this for initialization
    void Start () {
        beatTimer = FindObjectOfType<BeatTimer>();
        controller = GetComponent<MovementPhysics>();
        sprite = GetComponent<SpriteRenderer>();
        gravity = -(2 * stats.jumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {

        if(controller.collisions.right)
        {
            direction = -1.0f;
            sprite.flipX = true;
        }

        if (controller.collisions.left)
        {
            direction = 1.0f;
            sprite.flipX = false;
        }

        if (controller.collisions.below || controller.collisions.above) 
        {
            velocity.y = 0;
        }

        float targetVelocityX = 0;
        if (!stats.isStunned) 
        {
            targetVelocityX = stats.moveSpeed * direction;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < gravity)
        {
            velocity.y = gravity;
        }

        if (stats.force.x != 0 || stats.force.y != 0)
        {
            velocity.x += stats.force.x;
            velocity.y += stats.force.y;

            //Force is always trying to get to 0
            if (stats.force.y < 0)
            {
                stats.force.y += stats.weight * Time.deltaTime * Mathf.Abs(stats.force.y);
            }
            else
            {
                stats.force.y -= stats.weight * Time.deltaTime * Mathf.Abs(stats.force.y);
            }

            if (Mathf.Abs(stats.force.y) < forceThreshold)
            {
                stats.force.y = 0;
            }

            if (stats.force.x < 0)
            {
                stats.force.x += stats.weight * Time.deltaTime * Mathf.Abs(stats.force.x);
            }
            else
            {
                stats.force.x -= stats.weight * Time.deltaTime * Mathf.Abs(stats.force.x);
            }

            if (Mathf.Abs(stats.force.x) < forceThreshold)
            {
                stats.force.x = 0;
            }
        }

        controller.Move(velocity * Time.deltaTime * stats.movementDampener);

    }
}
