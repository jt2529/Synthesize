using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atatch script to a GameObject and then connect an object extending Toggler to target.
// Entering the switch's trigger will toggle the connected object, On to Off or Off To On.

// See Orb Switch prefab for example.

public class ActiveSwitch : MonoBehaviour
{
    [SerializeField]
    public Toggler[] targets;
    private bool active = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (targets.Length == 1)
        {
            active = targets[0].isToggled();
            animator.SetBool("active", active);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        foreach (Toggler t in targets)
        {
            t.toggle();
        }

            if (active)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        


        animator.SetBool("active", active);
    }

}