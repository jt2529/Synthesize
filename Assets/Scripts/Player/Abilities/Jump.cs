using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// I have created Jump as an ability so that it can be further customized without complicating the Movement controller
// Currently the only ability behaviour used is putting the jump on a short cooldown to force a small delay between valid jump inputs.

public class Jump : Ability
{

    private MovementController playerMovement;
    private PlayerStats stats;

    public GameEventScriptableObject playerDropBegin;
    public GameEventScriptableObject playerDropEnd;

    public override void beginAbility()
    {
        UseCharge();
        playerMovement.Jump();
        BeginCooldown(stats.minDelayBeforeNextJump); // A very short cooldown (0.1 seconds default) before jump becomes available again.
                
    }

    // Currently unused by the default jump ability. 
    // In theory you could do something like BeginAbilityDuration() with a time equal to the player's time to the Apex 
    // of their jump (or some other point of the jump) and then do something here like hover or do a slam down.
    public override void endAbility()
    {
        RefreshAllCharges();
    }

    public override void reload()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); 

        playerMovement = GetComponent<MovementController>();
        stats = GetComponent<PlayerStats>();

        _maxCharges = stats.maxNumberofJumps;
        _currentCharges = _maxCharges;
        
    }

    // This ability causes the player to fall through one way platforms.
    public void Drop()
    {
        stats.SetIsDropping(true);
        playerMovement.RemoveCollisionWithOneWayObstacles();
        StartCoroutine(DropDelay(stats.minDelayBeforeNextJump));
    }

    protected IEnumerator DropDelay(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        StopDropping();
    }

    public void StopDropping()
    {
        stats.SetIsDropping(false);
        //playerMovement.ResumeCollisionWithOneWayObstacles();
        
    }

}
