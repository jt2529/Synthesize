using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchMoveState : PlayerGroundedState {
	public PlayerCrouchMoveState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName) {
	}

	public override void Enter() {
		base.Enter();
		playerStats.SetColliderHeight(playerStats.crouchColliderHeight);
	}

	public override void Exit() {
		base.Exit();
		playerStats.SetColliderHeight(playerStats.standColliderHeight);
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (!isExitingState) {
			playerStats?.SetVelocityX(playerStats.moveSpeed *playerStats.crouchSpeedDampener * Movement.facingDirection);
			Movement?.CheckIfShouldFlip(xInput);

			if (xInput == 0) {
				stateMachine.ChangeState(playerStats.CrouchIdleState);
			} else if (yInput != -1 && !isTouchingCeiling) {
				stateMachine.ChangeState(playerStats.MoveState);
			}
		}

	}
}
