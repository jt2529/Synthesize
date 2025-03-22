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
    private Vector2 direction;
    Vector3 velocity;
    MovementPhysics physics;
    private BoxCollider2D myCollider;
    Vector2 targetLocation;
    GameController gameController;
    [SerializeField]
    private float forceThreshold = .01f;

    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        physics = GetComponent <MovementPhysics>();
        gravity = -(2 * stats.jumpHeight) / Mathf.Pow(stats.timeToJumpApex, 2);
        jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.jumpHeight);
        if (moveTowardsObject == null)
        {
            moveTowardsObject = GameObject.FindGameObjectWithTag("Player");
        }
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

            if (canJump && targetLocation.y > myCollider.bounds.min.y && physics.collisions.below)
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

        /*if (!canFly)
        {
            
            velocity.y += gravity * Time.deltaTime;
        }

        else 
        {
            if (targetVelocityX > velocity.x)
            {
                velocity.x += Time.deltaTime * ((physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            }
            else 
            {
                velocity.x -= Time.deltaTime * ((physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            }

            if (targetVelocityY > velocity.y)
            {
                velocity.y += Time.deltaTime * ((physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            }
            else
            {
                velocity.y -= Time.deltaTime * ((physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            }
        }
        */

        if (!canFly)
        { 
            velocity.y += gravity * Time.deltaTime; 
            if (velocity.y < gravity)
            {
                velocity.y = gravity;
            }
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

        Vector2 movementVelocity = velocity;
        if (canFly)
        {
            movementVelocity.y *= stats.movementDampener;
            movementVelocity.x *= stats.movementDampener;
        }
        else
        {
            movementVelocity.x *= stats.movementDampener;
        }


        physics.Move(movementVelocity * Time.deltaTime);
    }

    private void FindTargetLocation() 
    {
        direction = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min - myCollider.bounds.min;
        Vector2 positionTo;
        if (direction.x >= 0)
        {
            positionTo.x = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.max.x;
            positionTo.y = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.y;
        }
        else
        {
            positionTo.x = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.x;
            positionTo.y = moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.y;
            //positionTo = new Vector2(moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.x, moveTowardsObject.GetComponent<BoxCollider2D>().bounds.min.y);
        }

        float distance = Vector2.Distance(positionTo, transform.position);
        if (Mathf.Abs(distance) > stats.aggroDistance)
        {
            stats.isStunned = true;
        }
        else
        {
            stats.isStunned = false;
        }

        if (direction.x >= 0)
        {
            targetLocation.x = positionTo.x + xDistanceFromObject;
        }
        else
        {
            targetLocation.x = positionTo.x - xDistanceFromObject;
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
