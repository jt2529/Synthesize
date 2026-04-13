using System.Collections;
using System.Collections.Generic;
using Synthesize.Weapons;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;
    private WeaponGenerator weaponGenerator;

    private int inputIndex;

    private bool canInterrupt;

    private bool checkFlip;

    public PlayerAttackState(
        PlayerStats playerStats,
        PlayerStateMachine stateMachine,
        string animBoolName,
        Weapon weapon,
        CombatInputs input
    ) : base(playerStats, stateMachine, animBoolName)
    {
        this.weapon = weapon;

        weaponGenerator = weapon.GetComponent<WeaponGenerator>();

        inputIndex = (int)input;

        weapon.OnUseInput += HandleUseInput;

        weapon.EventHandler.OnEnableInterrupt += HandleEnableInterrupt;
        weapon.EventHandler.OnFinish += HandleFinish;
        weapon.EventHandler.OnFlipSetActive += HandleFlipSetActive;
    }

    private void HandleFlipSetActive(bool value)
    {
        checkFlip = value;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var playerAbilitiesInputHandler = playerStats.GetComponent<PlayerAbilitiesInputHandler>();

        var attackInputs = playerAbilitiesInputHandler.AttackInputs;

        weapon.CurrentInput = attackInputs[inputIndex];

        if (!canInterrupt)
           return;

        if (attackInputs[0] || attackInputs[1])
        {
            isAbilityDone = true;
        }
    }
    private void HandleWeaponGenerating()
    {
        stateMachine.ChangeState(playerStats.IdleState);
    }

    public override void Enter()
    {
        base.Enter();

        weaponGenerator.OnWeaponGenerating += HandleWeaponGenerating;
        
        checkFlip = true;
        canInterrupt = false;

        weapon.Enter();
    }


    public override void Exit()
    {
        base.Exit();

        weaponGenerator.OnWeaponGenerating -= HandleWeaponGenerating;
        
        weapon.Exit();
    }

    public bool CanTransitionToAttackState() => weapon.CanEnterAttack;

    private void HandleEnableInterrupt() => canInterrupt = true;

    private void HandleUseInput() =>  playerStats.playerAbilitiesInputHandler.UseAttackInput(inputIndex);

    private void HandleFinish()
    {
        AnimationFinishTrigger();
        isAbilityDone = true;
    }
}