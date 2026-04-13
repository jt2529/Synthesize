using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState {
	private int amountOfJumpsLeft;

	public PlayerJumpState(PlayerStats playerStats, PlayerStateMachine stateMachine,  string animBoolName) : base(playerStats, stateMachine, animBoolName) {
		amountOfJumpsLeft = playerStats.numberOfJumps;
	}

	public override void Enter() {
		base.Enter();
		playerStats?.SetVelocityY(playerStats.maxJumpVelocity); 
		isAbilityDone = true;
		playerStats.playerPhysicsInputHandler.jumpBuffered = false;
		amountOfJumpsLeft--;
		playerStats.InAirState.SetIsJumping();
	}

	public bool CanJump() {
		if (amountOfJumpsLeft > 0) {
			return true;
		} else {
			return false;
		}
	}

	public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerStats.numberOfJumps;

	public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
