using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployPlatform : Toggler {

    private Animator animator;
    public bool deployed = false;
    //private EdgeCollider2D edge;
    private BoxCollider2D collider;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        //edge = GetComponent<EdgeCollider2D>();
        collider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        //edge.enabled = deployed;
        collider.enabled = deployed;
        animator.SetBool("deployed", deployed);
	}

    public override void toggle()
    {
        if (!deployed)
        {
            deployed = true;
            collider.enabled = true;
        }
        else
        {
            deployed = false;
            collider.enabled = false;
        }

        animator.SetBool("deployed", deployed);
    }

    public override bool isToggled()
    {
        return deployed;
    }
}
