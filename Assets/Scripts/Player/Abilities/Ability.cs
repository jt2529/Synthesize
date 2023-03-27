using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// All player abilities will inheirit this class. The base class will handle its own cooldown/charges
// Specialized ability behaviour will be added in each ability implementation. 
public abstract class Ability : MonoBehaviour, IAbility
{

    protected PlayerStats abilitySource;

    protected CoreStatScriptableObject abilityStat;

    protected GameEventScriptableObject beginAbilityEvent;
    protected GameEventScriptableObject endAbilityEvent;

    protected GameEventScriptableObject reloadEvent;

    protected bool abilityOnCooldown = false;
    protected float cooldownTime = 4;
    //protected float cooldownTime = 4;
    protected float cooldownTimeRemaining = 4;

    public abstract void beginAbility();
    public abstract void endAbility();
    public abstract void reload();


    protected virtual void BeginCooldown(float cooldownTime)
    {
        abilityOnCooldown = true;
        Debug.Log("Cooldown Begins");
        cooldownTimeRemaining = cooldownTime;
            
    }

    // Call this in FixedUpdate of the implementing class
    // Override for custom cooldown behaviour
    protected virtual void CalculateRemainingCooldown()
    {
        // If our ability is on cooldown, check if our cooldownTime has lapsed
        if (abilityOnCooldown)
        {
            if (cooldownTimeRemaining <= 0)
            {
                // If it has, the ability is no longer on cooldown, and we no longer need to update cooldownTimeRemaining
                abilityOnCooldown = false;
                Debug.Log("Cooldown Ends");
                return;
            }
            // CooldownTime has not lapsed, so continue counting down
            cooldownTimeRemaining -= Time.deltaTime;
        }
    }
}
