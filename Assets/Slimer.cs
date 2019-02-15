﻿using System.Collections;
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

    private float direction = 1.0f;

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

        if(controller.collisions.right)
        {
            direction = -1.0f;
        }

        if (controller.collisions.left)
        {
            direction = 1.0f;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, speed * direction, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}