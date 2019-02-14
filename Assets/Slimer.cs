using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slimer : MonoBehaviour {

    private MovementPhysics controller;
    private bool right;
    public float speed;
    float velocityXSmoothing;
    float gravity;

    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;

    Vector3 velocity;

    // Use this for initialization
    void Start () {
        controller = GetComponent<MovementPhysics>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {



        if (!controller.collisions.right)
        {
            Debug.Log(velocity.x);
        }

    }
}
