using Synthesize.CoreSystem;
using UnityEngine;

public class PlayerStunState : PlayerState
{
    private readonly MovementPhysics movement;

    public PlayerStunState(
        PlayerStats playerStats,
        PlayerStateMachine stateMachine,
        string animBoolName
    ) : base(playerStats, stateMachine, animBoolName)
    {
        movement = core.GetCoreComponent<MovementPhysics>();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        playerStats?.SetVelocityX(0f);

        if (Time.time >= startTime + playerStats.stunTime)
        {
            stateMachine.ChangeState(playerStats.IdleState);
        }
    }
}
