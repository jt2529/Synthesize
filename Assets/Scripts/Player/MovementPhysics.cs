using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementPhysics : MonoBehaviour
{

    private LayerMask collisionMask;
    private GameObject gameObjectHit;
    const float skinWidth = .023f;
    RaycastOrigins raycastOrigins;

    public int horizontalRayCount = 5;
    public int verticalRayCount = 5;
    float horizontalRaySpacing;
    float verticalRaySpacing;
    public Vector2 currentVelocity;

    public float maxClimbAngle = 60;
    public float maxDescendAngle = 75;

    BoxCollider2D myCollider;
    public CollisionInfo collisions;
    PushableBoxController pushableBoxController;

    // Use this for initialization
    void Awake()
    {
        myCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        collisionMask = LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles", "OneWayObstacles" );
    }


    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity, ref gameObjectHit);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity, ref gameObjectHit);
        }

        transform.Translate(velocity);
    }

    public void Move(Vector2 velocity, PlayerStats playerStats)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        if (playerStats.isDropping)
        {
            collisionMask = LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles");
        }
        
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        if (velocity.x != 0)
        {
            playerStats.objectTouchingLeft = null;
            playerStats.objectTouchingRight = null;
            if (velocity.x > 0)
            {
                HorizontalCollisions(ref velocity, ref playerStats.objectTouchingRight);
            }

            if (velocity.x < 0)
            {
                HorizontalCollisions(ref velocity, ref playerStats.objectTouchingLeft);
            }

        }
        if (velocity.y != 0)
        {
            playerStats.objectTouchingBelow = null;
            playerStats.objectTouchingAbove = null;
            if (velocity.y > 0)
            {
                VerticalCollisions(ref velocity, ref playerStats.objectTouchingAbove);
            }

            if (velocity.y < 0)
            {
                 VerticalCollisions(ref velocity, ref playerStats.objectTouchingBelow);
            }
            
        }
        transform.Translate(velocity);

        if (playerStats.isDropping)
        {
            collisionMask = LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles", "OneWayObstacles");
        }
    }

    public void HorizontalCollisions(ref Vector2 velocity, ref GameObject gameObjectHit)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        pushableBoxController = null;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            
            if (hit)
            {
                //Check if object is movable, if so, then push it at the user's velocity
                if ((LayerMask.GetMask("MovableObstacles") & (1 << hit.transform.gameObject.layer)) != 0)
                {
                    pushableBoxController = hit.transform.gameObject.GetComponent<PushableBoxController>();
                    if (pushableBoxController.isInteractable == false)
                    {
                        pushableBoxController.Push(velocity);
                    }
                }
                else {

                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                    if (i == 0 && slopeAngle <= maxClimbAngle)
                    {

                        if (collisions.descendingSlope)
                        {
                            collisions.descendingSlope = false;
                            velocity = collisions.velocityOld;
                        }

                        float distanceToSlopeStart = 0;
                        if (slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - skinWidth;
                            velocity.x -= distanceToSlopeStart * directionX;
                        }
                        ClimbSlope(ref velocity, slopeAngle);
                        velocity.x += distanceToSlopeStart * directionX;
                    }

                    if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        rayLength = hit.distance;
                        if (collisions.climbingSlope)
                        {
                            velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad * Mathf.Abs(velocity.x));
                        }

                        collisions.left = directionX == -1;
                        collisions.right = directionX == 1;
                        collisions.wallSlope = slopeAngle;
                    }
                }
                if (gameObjectHit == null)
                {
                    gameObjectHit = hit.transform.gameObject;
                }
                
            }
        }
    }

    public void VerticalCollisions(ref Vector2 velocity, ref GameObject gameObjectHit)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        currentVelocity = new Vector2(0, 0);
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                if ((LayerMask.GetMask("OneWayObstacles") & (1 << hit.transform.gameObject.layer)) == 0 || directionY != 1)   //Only collide above if it is not a one-way obstacle
                {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                    }

                    if (directionY == -1)
                    {
                        collisions.below = true;
                        if (hit.transform.gameObject.GetComponent<MovementPhysics>() != null)
                        {
                            currentVelocity = hit.transform.gameObject.GetComponent<MovementPhysics>().currentVelocity;
                        }

                    }
                    else if (directionY == 1)
                    {

                        collisions.above = true;
                    }
                }

                if (gameObjectHit == null)
                {
                    gameObjectHit = hit.transform.gameObject;
                }
            }
        }
        velocity += currentVelocity;

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }

    }

    void ClimbSlope(ref Vector2 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }

    }

    void DescendSlope(ref Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft.x = bounds.min.x;
        raycastOrigins.bottomLeft.y = bounds.min.y;
        raycastOrigins.bottomRight.x = bounds.max.x;
        raycastOrigins.bottomRight.y = bounds.min.y;
        raycastOrigins.topLeft.x = bounds.min.x;
        raycastOrigins.topLeft.y = bounds.max.y;
        raycastOrigins.topRight.x = bounds.max.x;
        raycastOrigins.topRight.y = bounds.max.y;

    }

    void CalculateRaySpacing()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld, wallSlope;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            wallSlope = 0;
        }
    }
}
