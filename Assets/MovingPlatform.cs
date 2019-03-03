using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public PlatformNodeManager nodeManager;

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

        controller.Move(Vector3.Lerp(transform.position, new Vector3(rightTargetNode.transform.position.x, rightTargetNode.transform.position.x, transform.position.z), Time.deltaTime));	
	}



}
