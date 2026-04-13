using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState {

	private float velocityXSmoothing;
	public PlayerWallSlideState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
	{

	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (!isExitingState)
		{
			float targetVelocityX = xInput * playerStats.GetMoveSpeed();
			playerStats?.SetVelocityX(Mathf.SmoothDamp(playerStats.currentVelocityInput.x, targetVelocityX, ref velocityXSmoothing, playerStats.accelerationTimeGrounded));
			playerStats.currentVelocityInput.y += playerStats.gravity * Time.deltaTime;
			if (playerStats.currentVelocityInput.y < playerStats.gravity)
			{
				playerStats.currentVelocityInput.y = playerStats.gravity;
			}
			if (playerStats.currentVelocityInput.y < 0)
			{
				playerStats.currentVelocityInput.y += playerStats.wallSlideSpeedDampener * Time.deltaTime;
			}

		}

	}
}
