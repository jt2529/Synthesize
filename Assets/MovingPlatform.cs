using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public PlatformNodeManager nodeManager;
    public bool movingRight;
    public Vector3 velocity;
    public float smoothTime;
    public float speed;

    private float velocityXSmoothing = 0.0f;
    private MovementPhysics controller;

    private GameObject leftTargetNode;
    private GameObject rightTargetNode;

	// Use this for initialization
	void Start () {
        controller = GetComponent<MovementPhysics>();
        leftTargetNode = nodeManager.leftNode;
        rightTargetNode = nodeManager.rightNode;
	}
	
	// Update is called once per frame
	void Update () {
        if (movingRight == true)
        {
            velocity.x = speed;
            if (transform.position.x >= rightTargetNode.transform.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            velocity.x = speed * -1; 
            if (transform.position.x <= leftTargetNode.transform.position.x)
            {
                movingRight = true;
            }
        }


        transform.Translate(velocity);
    }
}
