using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Atatch to a GameObject with a Trigger Collider and then connect it to an object that extends the Toggler
// class. Entering the trigger will call the connected object's toggle method, activating the object while 
// the player remains within the trigger. Leaving the trigger disables the object again.

// See Auto-Platform prefab for an example.

public class TriggerSwitch : MonoBehaviour {

    public Toggler[] targets;
    private Animator animator;

    private bool isOccupied;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isOccupied = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(Toggler t in targets)
        {
            t.toggle();
        }
        
        if(animator != null)
        {
            isOccupied = true;
            animator.SetBool("toggle", true);
        }
        
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (Toggler t in targets)
        {
            t.toggle();
        }

        if (animator != null)
        {
            isOccupied = false;
            animator.SetBool("toggle", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Toggler t in targets)
        {
            Gizmos.DrawLine(transform.position, t.transform.position);
        }

    }

}
