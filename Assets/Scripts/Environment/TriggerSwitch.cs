using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Atatch to a GameObject with a Trigger Collider and then connect it to an object that extends the Toggler
// class. Entering the trigger will call the connected object's toggle method, activating the object while 
// the player remains within the trigger. Leaving the trigger disables the object again.

// See Auto-Platform prefab for an example.

public class TriggerSwitch : MonoBehaviour {

    public Toggler target;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.toggle();
            animator.SetBool("toggle", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.toggle();
            animator.SetBool("toggle", false);
    }

}
