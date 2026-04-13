using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState {
	private float velocityXSmoothing;
	public PlayerMoveState(PlayerStats playerStats, PlayerStateMachine stateMachine,  string animBoolName) : base(playerStats, stateMachine, animBoolName) {
	}

	public override void DoChecks() {
		base.DoChecks();
	}

	public override void Enter() {
		base.Enter();
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

	}

	public override void PhysicsUpdate() {
		Movement?.CheckIfShouldFlip(xInput);
		float targetVelocityX = xInput * playerStats.GetMoveSpeed();
		if ((xInput > 0 && playerStats.currentSpeed.x > targetVelocityX * Time.deltaTime)
			|| (xInput < 0 && playerStats.currentSpeed.x < targetVelocityX * Time.deltaTime))
		{
			playerStats?.SetVelocityX(Mathf.SmoothDamp(playerStats.currentVelocityInput.x, targetVelocityX, ref velocityXSmoothing, playerStats.decelerationTimeGrounded));
		}
		else
		{
			playerStats?.SetVelocityX(Mathf.SmoothDamp(playerStats.currentVelocityInput.x, targetVelocityX, ref velocityXSmoothing, playerStats.accelerationTimeGrounded));
		}

		if (!isExitingState)
		{
			if (xInput == 0)
			{
				stateMachine.ChangeState(playerStats.IdleState);
			}
		}
		base.PhysicsUpdate();
	}
}
