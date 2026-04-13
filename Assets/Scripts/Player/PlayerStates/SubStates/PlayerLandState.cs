using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(playerStats.MoveState);
            }
            else
            {
                stateMachine.ChangeState(playerStats.IdleState);
            }
        }       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
