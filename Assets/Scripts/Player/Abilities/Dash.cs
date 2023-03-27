using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dash is the first example of a player ability using the new Ability Abstract class
// It is here that we will begin interacting with the player and environment
// Logic such as cooldown times, charges remaining, and whether the ability is in a disabled state should be handled in the abstract class.
// Conditions for changing the ability's state are checked for here, but should modified by the Ability class.

public class Dash : Ability
{

    public new float cooldownTime;
    public new bool abilityOnCooldown = false;

    [Space(5)]
    [Header("Events")]
    public new GameEventScriptableObject beginAbilityEvent;
    public new GameEventScriptableObject endAbilityEvent;

    private PlayerStats stats;

    private void FixedUpdate()
    {
        CalculateRemainingCooldown();
    }

    public override void beginAbility()
    {
        Debug.Log("Dash Begin Primary");
        BeginCooldown(cooldownTime);
    }

    public override void endAbility()
    {
        Debug.Log("Dash End Primary");
    }

    public override void reload()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        cooldownTime = stats.dashChargeCooldownTime;
    }


}
