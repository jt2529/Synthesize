using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// All player abilities will inheirit this class. The base class will handle its own cooldown/charges
// Specialized ability behaviour will be added in each ability implementation. 
public abstract class Ability : MonoBehaviour, IAbility
{

    protected PlayerStats _abilitySource;

    protected CoreStatScriptableObject _abilityStat;

    [SerializeField] protected GameEventScriptableObject _beginAbilityEvent;
    [SerializeField] protected GameEventScriptableObject _endAbilityEvent;

    // Duration
    protected bool _abilityActive;
    protected float _abilityMaxDuration;
    protected float _abilityDurationRemaining;


    // I'm focusing on just getting a standard ability to work for now, where the ability is activated, lasts for a certain duration, and then deactivates. 
    // These variables are mostly placeholders for when I can work on a charge system in the future.
    // Some basic methods related to charges have been written but are mostly unused.
    // Charges
    // [SerializeField] protected bool _usesCharges = false;
    protected int _maxCharges;
    protected int _currentCharges;
    protected float _chargeMaxCooldownTime;
    protected float _chargeCurrentCooldownTime;

    //Cooldown
    protected bool _abilityOnCooldown = false;
    protected float _cooldownTime = 4;
    protected float _cooldownTimeRemaining = 4;

    public abstract void beginAbility();
    public abstract void endAbility();
    public abstract void reload();




    protected virtual void BeginCooldown(float cooldownTime)
    {
        StartCoroutine(CooldownTimer(cooldownTime));            
    }

    protected virtual void BeginabilityDurationTimer(float abilityDuration)
    {
        StartCoroutine(AbilityDurationTimer(abilityDuration));
    }

    protected virtual void BeginChargeCooldown(float chargeCooldownTime)
    {
        StartCoroutine(ChargeCooldownTimer(chargeCooldownTime));
    }


    protected virtual void AddCharge()
    {
        if (_currentCharges < _maxCharges)
        {
            _currentCharges += 1;
        }
    }

    protected virtual void UseCharge()
    {
        if (_currentCharges > 0)
        {
            _currentCharges -= 1;
        }
    }

    protected IEnumerator CooldownTimer(float cooldownTime)
    {
        _abilityOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        _abilityOnCooldown = false;
    }

    protected IEnumerator AbilityDurationTimer(float durationTime)
    {
        _abilityActive = true;
        yield return new WaitForSeconds(durationTime);
        _abilityActive = false;
        endAbility();
    }

    protected IEnumerator ChargeCooldownTimer(float chargeCooldownTime)
    {
        yield return new WaitForSeconds(chargeCooldownTime);
        AddCharge();
    }
}
