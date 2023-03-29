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

    // Charges (There are functions made to interact with charges, but the Ability class does not currently use charges when using an Ability.)
    [SerializeField] protected bool _usesCharges = false;
    protected int _maxCharges;
    protected int _currentCharges;
    protected float _chargeMaxCooldownTime;
    protected float _chargeCurrentCooldownTime;

    //Cooldown
    protected bool _abilityOnCooldown = false;
    protected bool _chargeIsRefreshing = false;
    protected bool _multipleChargeRefreshAllowed = false;
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

    protected virtual void BeginChargeCooldown(float chargeRefreshTime)
    {
        if (!_chargeIsRefreshing || _multipleChargeRefreshAllowed)
        {
            StartCoroutine(ChargeCooldownTimer(chargeRefreshTime));
        }
        
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
            BeginChargeCooldown(_cooldownTime);
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

    protected IEnumerator ChargeCooldownTimer(float chargeRefreshTime)
    {
        _chargeIsRefreshing = true;
        yield return new WaitForSeconds(chargeRefreshTime);
        _chargeIsRefreshing = false;
        AddCharge();
    }

    public virtual bool isOnCooldown()
    {
        return _abilityOnCooldown;
    }

    public virtual bool isActive()
    {
        return _abilityActive;
    }
}
