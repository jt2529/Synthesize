using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using TMPro;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    protected PlayerStats playerStats;
    protected PlayerStateMachine stateMachine;
    protected MovementPhysics movementPhysics;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;

    public PlayerState(PlayerStats playerStats, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.playerStats = playerStats;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = playerStats.Core;
        movementPhysics = core.GetCoreComponent<MovementPhysics>();
    }

    public virtual void Enter()
    {
        DoChecks();
        playerStats.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        playerStats.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        movementPhysics.Move(playerStats.currentVelocityInput * Time.deltaTime, playerStats);
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
    

}
