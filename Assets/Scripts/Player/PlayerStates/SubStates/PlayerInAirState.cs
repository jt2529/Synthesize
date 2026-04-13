using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    protected MovementPhysics Movement
    {
        get => movement ?? core.GetCoreComponent(ref movement);
    }

    private MovementPhysics movement;

    //Input
    private float xInput;
    private float velocityXSmoothing;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingCeiling;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;
    public bool isJumping;

    public PlayerInAirState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName) : base(playerStats, stateMachine, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (playerStats)
        {
            isGrounded = Movement.collisions.below;
            isTouchingCeiling = Movement.collisions.above;
            if (Movement.facingDirection > 0)
            {
                isTouchingWall = Movement.collisions.right;
                isTouchingWallBack = Movement.collisions.left;
            }
            else
            {
                isTouchingWall = Movement.collisions.left;
                isTouchingWallBack = Movement.collisions.right;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        xInput = playerStats.physicsInputHandler.hInput;
        jumpInput = playerStats.physicsInputHandler.jumpBuffered;
        jumpInputStop = playerStats.physicsInputHandler.jumpReleaseBuffered;
        dashInput = playerStats.physicsInputHandler.dashBuffered;

        CheckJumpMultiplier();

        if (playerStats.abilitiesInputHandler.AttackInputs[(int)CombatInputs.primary] && playerStats.PrimaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(playerStats.PrimaryAttackState);
        }
        else if (playerStats.abilitiesInputHandler.AttackInputs[(int)CombatInputs.secondary] && playerStats.SecondaryAttackState.CanTransitionToAttackState())
        {
            stateMachine.ChangeState(playerStats.SecondaryAttackState);
        }
        else if (isGrounded)
        {
            stateMachine.ChangeState(playerStats.LandState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack))
        {
            playerStats.WallJumpState.DetermineWallJumpDirection();
            stateMachine.ChangeState(playerStats.WallJumpState);
        }
        else if (jumpInput && playerStats.JumpState.CanJump())
        {
            stateMachine.ChangeState(playerStats.JumpState);
        }
        else if (isTouchingWall && xInput == Movement?.facingDirection)
        {
            stateMachine.ChangeState(playerStats.WallSlideState);
        }
        else if (dashInput && playerStats.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(playerStats.DashState);
        }
        else
        {
            
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                if (playerStats.currentVelocityInput.y > playerStats.minJumpVelocity)
                {
                    playerStats?.SetVelocityY(playerStats.minJumpVelocity);
                }
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        Movement?.CheckIfShouldFlip(xInput);
        //playerStats?.SetVelocityX(playerStats.moveSpeed * xInput);

        //playerStats.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        // playerStats.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if (isTouchingCeiling && playerStats.currentVelocityInput.y > 0)
        {
            playerStats.currentVelocityInput.y = 0;
        }
        float targetVelocityX = xInput * playerStats.GetMoveSpeed();
        // If currently moving faster than the intended movement speed, use half of acceleration time airborne, else use acceleration time airborne
        if ((xInput > 0 && playerStats.currentSpeed.x > targetVelocityX * Time.deltaTime)
            || (xInput < 0 && playerStats.currentSpeed.x < targetVelocityX * Time.deltaTime))
        {
            playerStats?.SetVelocityX(Mathf.SmoothDamp(playerStats.currentVelocityInput.x, targetVelocityX, ref velocityXSmoothing, playerStats.decelerationTimeAirborne));
            //targetVelocityX = playerStats.currentSpeed.x * (1 / Time.deltaTime);
        }
        else
        {
            playerStats?.SetVelocityX(Mathf.SmoothDamp(playerStats.currentVelocityInput.x, targetVelocityX, ref velocityXSmoothing, playerStats.accelerationTimeAirborne));
        }


        playerStats.currentVelocityInput.y += playerStats.gravity * Time.deltaTime;
        if (playerStats.currentVelocityInput.y < playerStats.gravity)
        {
            playerStats.currentVelocityInput.y = playerStats.gravity;
        }
        base.PhysicsUpdate();
    }

    public void SetIsJumping() => isJumping = true;
}