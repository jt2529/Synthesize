using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerGroundedState {
	public PlayerCrouchIdleState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName) {
	}

	public override void Enter() {
		base.Enter();

		//playerStats?.SetVelocityZero();
		playerStats.SetColliderHeight(playerStats.crouchColliderHeight);
	}

	public override void Exit() {
		base.Exit();
		playerStats.SetColliderHeight(playerStats.standColliderHeight);
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (!isExitingState) {
			if (xInput != 0) {
				stateMachine.ChangeState(playerStats.CrouchMoveState);
			} else if (yInput != -1 && !isTouchingCeiling) {
				stateMachine.ChangeState(playerStats.IdleState);
			}
		}
	}
}
