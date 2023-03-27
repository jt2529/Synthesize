using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    // Start is called before the first frame update

    public bool jumpActivated;
    public float jumpPadPower;

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jumpActivated != true)
        {
            PlayerMovementController playerPhysicsController = (PlayerMovementController)collision.GetComponentInParent(typeof(PlayerMovementController));
            if (playerPhysicsController)
            {
                playerPhysicsController.forceUpward = jumpPadPower;
                PlayerStats playerStats = (PlayerStats)collision.GetComponentInParent(typeof(PlayerStats));
                playerStats.numberOfJumpsLeft = playerStats.maxNumberofJumps - 1;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (jumpActivated == true)
        {
            PlayerMovementController playerPhysicsController = (PlayerMovementController)collision.GetComponentInParent(typeof(PlayerMovementController));
            if (playerPhysicsController)
            {
                if (playerPhysicsController.getJumpBuffered()) ;
                playerPhysicsController.forceUpward = jumpPadPower;
            }
        }
    }

}
