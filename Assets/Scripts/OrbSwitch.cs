using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSwitch : MonoBehaviour
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