using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerPhysicsController))]
public class PlayerPhysicsController : MonoBehaviour {

    PlayerStats stats;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    SpriteRenderer sprite;

    float velocityXSmoothing;

    PlayerPhysics physics;

	// Use this for initialization
	void Start () {
        physics = GetComponent<PlayerPhysics>();
        stats = GetComponent<PlayerStats>();
        gravity = -(2 * stats.GetMaxJumpHeight()) / Mathf.Pow(stats.GetTimeToJumpApex(), 2);
        maxJumpVelocity = Mathf.Abs(gravity) * stats.GetTimeToJumpApex();
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * stats.GetMinJumpHeight());
        sprite = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

        if (physics.collisions.above || physics.collisions.below) {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Set the player's direction (true for right, false for left)
        if (input.x > 0) {
            stats.SetPlayerDirection(true);
            sprite.flipX = false;
        }

        if (input.x < 0) {
            stats.SetPlayerDirection(false);
            sprite.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && physics.collisions.below) {
            velocity.y = maxJumpVelocity;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }

        float targetVelocityX = input.x * stats.GetMoveSpeed();
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (physics.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        physics.Move(velocity * Time.deltaTime);
	}
}
