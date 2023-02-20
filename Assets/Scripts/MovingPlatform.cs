using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float speed;
    public float pointProximity;
    public int startingPoint;
    public bool movesCarriedObjects;
    public Transform[] points;
    private MovementPhysics movement;
    private Vector2 velocity;

    private int i;

	// Use this for initialization
	void Start () {
        movement = GetComponent<MovementPhysics>();
        transform.position = points[startingPoint].position;
    }



    // Update is called once per frame
    void FixedUpdate () {
        if (Vector2.Distance(transform.position, points[i].position) < pointProximity) 
        {
            i++;
            if (i == points.Length) 
            {
                i = 0;
            }
            
        }

        velocity = (points[i].position - transform.position).normalized;
        velocity *= speed * Time.deltaTime;
        transform.Translate(velocity); // movement.Move(velocity);
        if (movesCarriedObjects) 
        { 
            movement.currentVelocity = velocity;
        }
    }
}

