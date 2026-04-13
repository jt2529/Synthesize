using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState {

	protected MovementPhysics Movement { get => movement ?? core.GetCoreComponent(ref movement); }

	private MovementPhysics movement;


	protected bool isGrounded;
	protected bool isTouchingWall;
	protected bool jumpInput;
	protected float xInput;
	protected float yInput;

	public PlayerTouchingWallState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
	{
	}

	public override void AnimationFinishTrigger() {
		base.AnimationFinishTrigger();
	}

	public override void AnimationTrigger() {
		base.AnimationTrigger();
	}

	public override void DoChecks() {
		base.DoChecks();

		isGrounded = Movement.collisions.below;
		if (Movement.facingDirection > 0)
		{

			isTouchingWall = Movement.collisions.right;
		}
		else
		{
			isTouchingWall = Movement.collisions.left;
		}
	}

	public override void Enter() {
		base.Enter();
	}

	public override void Exit() {
		base.Exit();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		xInput = playerStats.physicsInputHandler.hInput;
		yInput = playerStats.physicsInputHandler.vInput;
		jumpInput = playerStats.physicsInputHandler.jumpBuffered;

		if (jumpInput)
		{
			playerStats.WallJumpState.DetermineWallJumpDirection();
			stateMachine.ChangeState(playerStats.WallJumpState);
		}
		else if (isGrounded)
		{
			stateMachine.ChangeState(playerStats.IdleState);
		}
		else if (!isTouchingWall)
		{
			stateMachine.ChangeState(playerStats.InAirState);
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
