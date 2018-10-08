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
        if (animator != null && toggled)
        {
            animator.SetBool("toggle", toggled);

            if (col != null)
            {
                col.enabled = false;
            }
        }

        if (animator != null && !toggled)
        {
            animator.SetBool("toggle", toggled);

            if (col != null)
            {
                col.enabled = true;
            }
        }
    }

    public override void toggle()
    {
        if (!toggled)
        {
            toggled = true;
        }else
        {
            toggled = false;
        }
        
    }
}
