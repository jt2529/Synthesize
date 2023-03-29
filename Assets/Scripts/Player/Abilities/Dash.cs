using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Dash is the first example of a player ability using the new Ability Abstract class
public class Dash : Ability
{

    public bool playerInvulnerableWhileDashing = true;

    private PlayerStats stats;
    private PlayerInputController playerInput;

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
        if(!_abilityActive && !_abilityOnCooldown)
        {
            BeginabilityDurationTimer(_abilityMaxDuration);
            BeginCooldown(_cooldownTime);

            if (playerInvulnerableWhileDashing)
            {
                stats.playerInvulnerable = true;
            }
        }
    }

    public override void endAbility()
    {
        stats.isDashingEnd = true;

        if (playerInvulnerableWhileDashing)
        {
            stats.playerInvulnerable = false;
        }
    }

    // This is unused by the new dash/Ability class but is left here for now because older scripts use it.
    // I will fix this soon.
    public override void reload()
    {
        if(stats.numberOfDashesLeft < stats.numberOfDashes)
        {
            stats.numberOfDashesLeft += 1;
        }
    }

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        playerInput = GetComponent<PlayerInputController>();
        _cooldownTime = stats.dashChargeCooldownTime;
        _abilityMaxDuration = stats.fullDashTime;
    }
    
}

    



