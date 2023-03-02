using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicsController : MonoBehaviour {
    public EnemyStats stats;
    public GameObject moveTowardsObject;
    public float xDistanceFromObject = 3f;
    public float yDistanceFromObject = 1f;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;
    public bool canFly;
    public bool canJump;
    public float jumpInterval;
    public float gravity;
    public float targetVerticalOffset;
    float jumpVelocity; 
    float velocityXSmoothing;
    float velocityYSmoothing;
    Vector3 velocity;
    MovementPhysics physics;
    BoxCollider2D collider;
    Vector2 targetLocation;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider2D>();
        physics = GetComponent <MovementPhysics>();
        gravity = -(2 * stats.jumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.jumpHeight);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        FindTargetLocation();
        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
            stats.force.y = 0;
        }
        float targetVelocityX = 0;
        float targetVelocityY = 0;

        if (!stats.isStunned) 
        {
            if (transform.position.x > targetLocation.x)
            {
                targetVelocityX = (-1) * stats.moveSpeed;
            }
            else if (transform.position.x < targetLocation.x)
            {
                targetVelocityX = stats.moveSpeed;
            }

            if (canJump && targetLocation.y < collider.bounds.min.y && physics.collisions.below)
            {
                velocity.y = jumpVelocity;
            }

            if (canFly) 
            {
                if (transform.position.y > targetLocation.y)
                {
                    targetVelocityY = (-1) * stats.moveSpeed;
                }
                else if (transform.position.y < targetLocation.y)
                {
                    targetVelocityY = stats.moveSpeed;
                }
            }
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeAirborne);

        if (canFly != true)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (velocity.y < gravity)
        {
            velocity.y = gravity;
        }

        //Add current force to velocity, and decrement the force stat value by object weight * time
        if (stats.force.x != 0 || stats.force.y != 0)
        {
            velocity.x += stats.force.x;
            velocity.y += stats.force.y;
            
            //Force is always trying to get to 0
            if (stats.force.y < 0)
            {
                stats.force.y += stats.weight * Time.deltaTime * Mathf.Abs(stats.force.y);
                if (stats.force.y > 0) 
                {
                    stats.force.y = 0;
                }
            }
            else 
            {
                stats.force.y -= stats.weight * Time.deltaTime * Mathf.Abs(stats.force.y);
                if (stats.force.y < 0)
                {
                    stats.force.y = 0;
                }
            }

            if (stats.force.x < 0)
            {
                stats.force.x += stats.weight * Time.deltaTime * Mathf.Abs(stats.force.x);
                if (stats.force.x > 0)
                {
                    stats.force.x = 0;
                }
            }
            else
            {
                stats.force.x -= stats.weight * Time.deltaTime * Mathf.Abs(stats.force.x);
                if (stats.force.x < 0)
                {
                    stats.force.x = 0;
                }
            }
        }

        physics.Move(velocity * Time.deltaTime);
    }

    private void FindTargetLocation() {
        Vector2 direction = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min - collider.bounds.min;
        Vector2 positionTo;
        if (direction.x >= 0)
        {
            positionTo = new Vector2(moveTowardsObject.GetComponent<BoxCollider2D>().bounds.max.x, moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.y);
        }
        else
        {
            positionTo = new Vector2(moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.x, moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.y);
        }
        
        if (direction.x >= 0)
        {
            targetLocation.x = positionTo.x - xDistanceFromObject;
        }
        else
        {
            targetLocation.x = positionTo.x + xDistanceFromObject;
        }
        if (direction.y >= 0)
        {
            targetLocation.y = positionTo.y - yDistanceFromObject + targetVerticalOffset;
        }
        else
        {
            targetLocation.y = positionTo.y + yDistanceFromObject + targetVerticalOffset;
        }
    }
}
