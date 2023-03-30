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

    private int i = 0;

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


    private void OnDrawGizmosSelected()
    {
        // Draws a line in the Editor between all points in the array to visually indicate where the platform will move to.
        for (i = 0; i < points.Length - 1; i++)
        {
            Gizmos.DrawLine(points[i].transform.position, points[i + 1].transform.position);
        }
        Gizmos.DrawLine(points[points.Length - 1].transform.position, points[0].transform.position);
    }
}

