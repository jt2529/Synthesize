using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployPlatform : MonoBehaviour, IToggleable {

    private Animator animator;
    public bool deployed = false;
    private EdgeCollider2D edge;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        edge = GetComponent<EdgeCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void togglePlatform()
    {

    }

    public void toggle()
    {
        if (!deployed)
        {
            deployed = true;
            edge.enabled = true;
        }
        else
        {
            deployed = false;
            edge.enabled = false;
        }

        animator.SetBool("deployed", deployed);
    }
}
