using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformNodeManager : MonoBehaviour {

    public GameObject leftNode;
    public GameObject rightNode;

    private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, leftNode.transform.position);
        lineRenderer.SetPosition(1, rightNode.transform.position);
    }
}
