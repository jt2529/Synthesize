using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using UnityEngine;

public class PlayerAbilityState : PlayerState {
	protected bool isAbilityDone;

	protected MovementPhysics Movement { get => movement ?? core.GetCoreComponent(ref movement); }

	private MovementPhysics movement;

	public bool isGrounded;

	public PlayerAbilityState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName){
	}

	public override void DoChecks() {
		base.DoChecks();
		isGrounded = Movement.collisions.below;
	}

	public override void Enter() {
		base.Enter();

		isAbilityDone = false;
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

 	if (isAbilityDone) {
			if (playerStats.playerPhysicsInputHandler.jumpBuffered && playerStats.JumpState.CanJump()) 
			{
				stateMachine.ChangeState(playerStats.JumpState);
			}
			else if (isGrounded)  //playerStats.currentVelocityInput.y < 0.01f && playerStats.) {
				if (playerStats.playerPhysicsInputHandler.hInput == 0)
				{
					stateMachine.ChangeState(playerStats.IdleState);
				}
				else 
				{
					stateMachine.ChangeState(playerStats.MoveState);
				}
			else 
			{
				stateMachine.ChangeState(playerStats.InAirState);
			}
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
