using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected float xInput;
    protected float yInput;

    protected bool isTouchingCeiling;

    protected MovementPhysics Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    protected MovementPhysics movement;

   // private MovementPhysics movement;

    private bool jumpInput;
    private bool dropInput;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool dashInput;

    public PlayerGroundedState(PlayerStats playerStats, PlayerStateMachine stateMachine,
        string animBoolName) : base(playerStats, stateMachine, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (playerStats)
        {
            isGrounded = Movement.collisions.below;
        }
    }

    public override void Enter()
    {
        base.Enter();
        playerStats.JumpState.ResetAmountOfJumpsLeft();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = playerStats.playerPhysicsInputHandler.hInput;
        yInput = playerStats.playerPhysicsInputHandler.vInput;
        jumpInput = playerStats.playerPhysicsInputHandler.jumpBuffered ;
        dropInput = playerStats.playerPhysicsInputHandler.dropBuffered;
        dashInput = playerStats.playerPhysicsInputHandler.dashBuffered;

        if (playerStats.playerAbilitiesInputHandler.AttackInputs[(int)CombatInputs.primary] && playerStats.PrimaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(playerStats.PrimaryAttackState);
        }
        else if (playerStats.playerAbilitiesInputHandler.AttackInputs[(int)CombatInputs.secondary] && playerStats.SecondaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(playerStats.SecondaryAttackState);
        }
        else if (jumpInput && playerStats.JumpState.CanJump() && !isTouchingCeiling)
        {
            stateMachine.ChangeState(playerStats.JumpState);
        }
        else if (!isGrounded)
        {
            stateMachine.ChangeState(playerStats.InAirState);
        }
        else if (dashInput && playerStats.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(playerStats.DashState);
        }

        

    }

    public override void PhysicsUpdate()
    {
        if (!playerStats.InAirState.isJumping)
        {
            playerStats?.SetVelocityY(0);
        }
        playerStats.currentVelocityInput.y += playerStats.gravity * Time.deltaTime;
        if (playerStats.currentVelocityInput.y < playerStats.gravity)
        {
            playerStats.currentVelocityInput.y = playerStats.gravity;
        }
        base.PhysicsUpdate();
    }
}