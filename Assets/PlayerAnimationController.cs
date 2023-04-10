using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if(stats.PlayerFacingDirection() < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        animator.SetBool("isAlive", stats.isPlayerAlive());
        animator.SetBool("isDashing", stats.isDashing);
        animator.SetBool("isGrounded", stats.isGrounded);
        animator.SetBool("isRunning", stats.horizontalInput);
        animator.SetBool("WallSlide", stats.IsWallSliding());
            
    }
}
