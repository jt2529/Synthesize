using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atatch script to a GameObject and then connect an object extending Toggler to target.
// Entering the switch's trigger will toggle the connected object, On to Off or Off To On.

// See Orb Switch prefab for example.

public class ActiveSwitch : MonoBehaviour
{

    public Toggler target;
    private bool active = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Switch toggled");
        target.toggle();

        if (active)
        {
            active = false;
        } else
        {
            active = true;
        }

        animator.SetBool("active", active);
    }

}