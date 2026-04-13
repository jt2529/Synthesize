using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.InputSystem;
using Synthesize.CoreSystem;

public class PlayerDashState : PlayerAbilityState {
	private Vector2 dashDirection;
	public int numberOfDashesLeft;
	private float dashTimeLeft;
	public float dashRecoveryStartTime = 0f;
	public float xInput;
	public float yInput;
	public bool jumpBuffered;
	public float fixedYInputAngle = .3f;

	public PlayerDashState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
	{
	}
	public override void Enter() {
		base.Enter();
		numberOfDashesLeft--;
		playerStats.playerPhysicsInputHandler.dashBuffered = false;
 		xInput = Movement.facingDirection;
		yInput = playerStats.physicsInputHandler.vInput;
		if (yInput < 0)
		{
			yInput = fixedYInputAngle * -1;
		}
		else 
		{
			yInput = 0;
		}
		dashDirection = new Vector2(xInput, yInput);
		dashTimeLeft = playerStats.fullDashTime;
		if (dashRecoveryStartTime == 0)  //Only set dash recovery start time if it isn't already being recovered (when value is already set)
		{ 
			dashRecoveryStartTime = Time.time;
		}
	}

	public override void Exit() {
		base.Exit();

	}

	public override void LogicUpdate() {
		base.LogicUpdate();
		dashTimeLeft -= Time.deltaTime;
		jumpBuffered = playerStats.physicsInputHandler.jumpBuffered;
		if (!isExitingState)
		{
			playerStats.SetVelocity(dashDirection * playerStats.dashSpeedMultiplier * playerStats.GetMoveSpeed());
			if (jumpBuffered) 
			{
				if (isGrounded)
				{
					playerStats.SetVelocityX(dashDirection.x * playerStats.dashSpeedMultiplier * playerStats.GetMoveSpeed());
					playerStats.JumpState.ResetAmountOfJumpsLeft();
				}
				else 
				{
					playerStats.SetVelocityX(xInput * playerStats.GetMoveSpeed());
				}
				playerStats.SetVelocityY(playerStats.maxJumpVelocity);
				stateMachine.ChangeState(playerStats.JumpState);
				dashTimeLeft = 0;
				isAbilityDone = true;
			}
			else if (isGrounded)
			{
				playerStats.SetVelocityY(playerStats.gravity * Time.deltaTime);
			}
		}
		

			
		if (dashTimeLeft <= 0 && !jumpBuffered)
		{
			playerStats.SetVelocityX(xInput * playerStats.GetMoveSpeed());
			dashTimeLeft = 0;
			isAbilityDone = true;
		}
	}

	public void DashChargeRecovery()
	{
		if (numberOfDashesLeft < playerStats.numberOfDashes)
		{
			numberOfDashesLeft++;
		}
	}

	public bool CheckIfCanDash() {
		return (numberOfDashesLeft > 0);
	}

	public void ResetCanDash() 
	{
		numberOfDashesLeft = playerStats.numberOfDashes;
	} 
}
