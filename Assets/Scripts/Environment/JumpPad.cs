using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Synthesize.CoreSystem;
using Synthesize.Combat.KnockBack;

public class JumpPad : MonoBehaviour
{
    // Start is called before the first frame update

    public bool jumpActivated;
    public Vector2 angle;
    public float jumpPadPower;
    private Core core;

    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (jumpActivated != true)
       {

            if (core = collision.gameObject.GetComponentInChildren<Core>())
            {
                core.GetCoreComponent<KnockBackReceiver>().KnockBack(new KnockBackData(angle, jumpPadPower, core.GetCoreComponent<MovementPhysics>().facingDirection, this.gameObject));
            };
            /*
            PlayerPhysicsInputHandler playerPhysicsInputHandler = (PlayerPhysicsInputHandler)collision.GetComponentInParent(typeof(PlayerPhysicsInputHandler));
            if (playerPhysicsInputHandler)
            {
                playerPhysicsInputHandler.forceUpward = jumpPadPower;
                PlayerStats playerStats = (PlayerStats)collision.GetComponentInParent(typeof(PlayerStats));
                playerStats.numberOfJumpsLeft = playerStats.numberOfJumps - 1;
            }
            */
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (jumpActivated == true)
        {

            if (collision.TryGetComponent(out PlayerPhysicsInputHandler playerPhysicsInputHandler))
            {
                if (playerPhysicsInputHandler.getJumpBuffered())
                {
                    if (core = collision.gameObject.GetComponentInChildren<Core>())
                    {
                        core.GetCoreComponent<KnockBackReceiver>().KnockBack(new KnockBackData(angle, jumpPadPower, core.GetCoreComponent<MovementPhysics>().facingDirection, this.gameObject));
                        //core.GetCoreComponent<MovementPhysics>().ForceY = jumpPadYPower;
                        //core.GetCoreComponent<MovementPhysics>().ForceX = jumpPadXPower;
                    }
                    
                }
                
            }
            /*PlayerPhysicsInputHandler playerPhysicsInputHandler = (PlayerPhysicsInputHandler)collision.GetComponentInParent(typeof(PlayerPhysicsInputHandler));
            if (playerPhysicsInputHandler)
            {
                if (playerPhysicsInputHandler.getJumpBuffered())
                { 
                    playerPhysicsInputHandler.forceUpward = jumpPadPower; 
                }
                
            }
            */
        }
    }

}
