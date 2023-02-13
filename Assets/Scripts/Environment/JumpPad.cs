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
            PlayerPhysicsController playerPhysicsController = (PlayerPhysicsController)collision.GetComponentInParent(typeof(PlayerPhysicsController));
            if (playerPhysicsController) 
            {
                playerPhysicsController.forceUpward = jumpPadPower;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (jumpActivated == true)
        {
            PlayerPhysicsController playerPhysicsController = (PlayerPhysicsController)collision.GetComponentInParent(typeof(PlayerPhysicsController));
            if (playerPhysicsController)
            {
                if (playerPhysicsController.getJumpBuffered()) ;
                playerPhysicsController.forceUpward = jumpPadPower;
            }
        }
    }

}
