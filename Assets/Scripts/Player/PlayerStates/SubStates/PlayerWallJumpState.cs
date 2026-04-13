using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState {
	private int wallJumpDirection;

	public PlayerWallJumpState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
	{
	}

	public override void Enter() {
		base.Enter();
		playerStats.SetVelocityX(playerStats.wallJumpPower * wallJumpDirection);
		playerStats.SetVelocityY(playerStats.wallJumpHeight);
		Movement?.CheckIfShouldFlip(wallJumpDirection);
		playerStats.playerPhysicsInputHandler.jumpBuffered = false;
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		isAbilityDone = true;

		//playerStats.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		//playerStats.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
	}

	public void DetermineWallJumpDirection() {
		wallJumpDirection = -Movement.facingDirection;

	}
}
