﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class BulletPhysics : MonoBehaviour
{

    public LayerMask playerMask;
    public LayerMask obstacleMask;
    const float skinWidth = .015f;
    RaycastOrigins raycastOrigins;
    HarmfulObject harmfulObject;

    public int horizontalRayCount = 2;
    public int verticalRayCount = 2;
    float horizontalRaySpacing;
    float verticalRaySpacing;
    PlayerStats playerStats;

    BoxCollider2D collider;
    public CollisionInfo collisions;
    float speed;
    Vector2 direction;
    bool isReady;

    // Use this for setting variable values
    private void Awake()
    {
        speed = .055f;
        isReady = false;
    }

    // Use this for initialization
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();

        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        playerMask = LayerMask.GetMask("Player");
        obstacleMask = LayerMask.GetMask("Obstacles");
        harmfulObject = GetComponent<HarmfulObject>();
        
    }

    void Update() {


        if (isReady)
        {
            Vector2 velocity = transform.position;

            velocity.x = direction.x * speed;
            velocity.y = direction.y * speed;

            
            velocity += speed * direction * Time.deltaTime;
            Move(velocity);
        }

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        

        if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
        {
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, obstacleMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                Destroy(gameObject);
            }

            hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, playerMask);

            if (hit)
            {
                playerStats.ChangeHealth(-harmfulObject.damage);
                Destroy(gameObject);
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, obstacleMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                Destroy(gameObject);
            }

            hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, playerMask);
            if (hit)
            {
                playerStats.ChangeHealth(-harmfulObject.damage);
                Destroy(gameObject);
            }
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
