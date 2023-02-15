using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletPhysics : MonoBehaviour
{

    const float skinWidth = .015f;
    RaycastOrigins raycastOrigins;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    public HarmfulObject harmfulObject;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    public LayerMask obstacleMask;
    public LayerMask breakableMask;

    public int horizontalRayCount = 2;
    public int verticalRayCount = 2;
    public bool playerBullet;

    public BoxCollider2D collider;
    public CollisionInfo collisions;
    public Vector2 playerSpeed;
    public float speed;
    public Vector2 direction;
    public bool isReady;

    public GameObject onDestroy;

    // Use this for setting variable values
    private void Awake()
    {
        isReady = false;
    }

    // Use this for initialization
    void Start()
    {
        CalculateRaySpacing();
        obstacleMask = LayerMask.GetMask("Breakables", "Obstacles", "MovingObstacles", "MovableObstacles");

    }

    void Update() {

        if (isReady)
        {
            Vector2 velocity = transform.position;

            velocity.x = direction.x * (speed * Time.deltaTime + playerSpeed.x * Time.deltaTime);
            velocity.y = direction.y * (speed * Time.deltaTime + playerSpeed.y * Time.deltaTime);

            Move(velocity);

        }
            
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));


        if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
        {
            Instantiate(onDestroy, transform);
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        SpriteRenderer img = GetComponent<SpriteRenderer>();
        direction = newDirection.normalized;
        if (direction.x < 0)
        {
            img.flipX = true;
        }
        isReady = true;
    }


    public void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        if (velocity.x != 0)
        {
            HorizontalCollisions(velocity);
        }

        if (velocity.y != 0)
        {
            VerticalCollisions(velocity);
        }
        transform.Translate(velocity);
    }

    void HorizontalCollisions(Vector2 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit;
            if (playerBullet) {
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, enemyMask);
                if (!hit)
                {
                    hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, breakableMask);
                }
            }
            else
            {
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, playerMask);
            }
            if (hit)
            {
                BulletHit(hit);
            }

            hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, obstacleMask);
            if (hit)
            {
                harmfulObject.tryDestroy();
            }
        }
    }

    void VerticalCollisions(Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit;
            if (playerBullet)
            {
                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, enemyMask);
                
            }
            else
            {
                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, playerMask);

            }

            if (!hit)
            {
                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, breakableMask);
            }

            if (hit)
            {
               BulletHit(hit);
            }

            hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, obstacleMask);
            if (hit)
            {
                harmfulObject.tryDestroy();
            }
        }
    }

    private void BulletHit(RaycastHit2D hit)
    {
       if (playerBullet)
        {   
            EnemyStats enemyStats = hit.collider.gameObject.GetComponent<EnemyStats>();
            if (enemyStats != null) 
            {
                enemyStats.ChangeHealth(-harmfulObject.damage);
                harmfulObject.tryDestroy();
            }
        }
        else
        {
            PlayerStats playerStats = hit.collider.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.ChangeHealth(-harmfulObject.damage);
                harmfulObject.tryDestroy();
            }
        }

        BreakableStats breakableStats = hit.collider.gameObject.GetComponent<BreakableStats>(); // Destroy enemyh bullets on collision with other enemy
        if (breakableStats != null)
        {
            if (playerBullet) 
            {
                breakableStats.ChangeHealth(-harmfulObject.damage);
            }
               
            harmfulObject.tryDestroy();
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
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

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}
