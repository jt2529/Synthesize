using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBoxController : MonoBehaviour {

    private MovementPhysics physics;
    private Vector2 velocity;

    [SerializeField]
    float gravity;

    // Use this for initialization
    void Start () {
        physics = GetComponent<MovementPhysics>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //velocity.x = 0;
        velocity.y += gravity * Time.deltaTime;
        physics.Move(velocity);
    }

    public void Push(Vector2 velocity)
    {

        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
        }

        //if (physics.collisions.left || physics.collisions.right)
        //{
        //    velocity.x = 0;
        //}
   
        physics.Move(velocity);
    }
}
