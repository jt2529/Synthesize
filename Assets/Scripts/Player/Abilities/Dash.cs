using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

// Dash is the first example of a player ability using the new Ability Abstract class
public class Dash : Ability
{

    public bool playerInvulnerableWhileDashing = true;
    private PlayerStats stats;

    private void FixedUpdate()
    {
        if (_abilityActive)
        {
            stats.setPlayerIsDashing();
        }else
        {
            stats.setPlayerNotDashing();
        }
         
    }

    public override void beginAbility()
    {
        if(!_abilityActive && !_abilityOnCooldown)
        {
            BeginAbilityDurationTimer(_abilityMaxDuration);
            BeginCooldown(_cooldownTime);

            _beginAbilityEvent.Raise();

            if (playerInvulnerableWhileDashing)
            {
                stats.playerInvulnerable = true;
            }
        }
    }

    public override void endAbility()
    {
        _endAbilityEvent.Raise();

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
        _cooldownTime = stats.dashChargeCooldownTime;
        _abilityMaxDuration = stats.fullDashTime;
    }
    
}

    



