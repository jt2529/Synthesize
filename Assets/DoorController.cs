using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Toggler {

    public bool toggled = false;
    private Animator animator;
    private BoxCollider2D col;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

        animator.SetBool("toggle", toggled);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void toggle()
    {
        if (!toggled)
        {
            Debug.Log("Door opened.");

            toggled = true;

            if (col != null)
            {
                col.enabled = false;
            }
            if(animator != null)
            {
                animator.SetBool("toggle", toggled);
            }
        }else
        {
            Debug.Log("Door closed.");
            toggled = false;

            if (col != null)
            {
                col.enabled = true;
            }
            if (animator != null)
            {
                animator.SetBool("toggle", toggled);
            }
        }
        
    }
}
