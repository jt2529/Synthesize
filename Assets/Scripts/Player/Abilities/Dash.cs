using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Dash is the first example of a player ability using the new Ability Abstract class

// There is no enforcement of dash eligibility. If it is off cooldown and you press dash, the player will atempt to dash. 
public class Dash : Ability
{

    private PlayerStats stats;
    private PlayerInputController playerInput;

    private float dashTimeRemaining;

    private void FixedUpdate()
    {
        if (_abilityActive)
        {
            stats.isDashing = true;
        }else
        {
            stats.isDashing = false;
        }
         
    }

    public override void beginAbility()
    {
        BeginabilityDurationTimer(_abilityMaxDuration);
        BeginCooldown(_cooldownTime);
    }

    public override void endAbility()
    {
        stats.isDashingEnd = true;
    }

    public override void reload()
    {
        if(stats.numberOfDashesLeft < stats.numberOfDashes)
        {
            stats.numberOfDashesLeft += 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerInput = GetComponent<PlayerInputController>();
        _cooldownTime = stats.dashChargeCooldownTime;
        _abilityMaxDuration = stats.fullDashTime;
    }
    
}

    



