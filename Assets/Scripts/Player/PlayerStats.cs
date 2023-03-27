using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable<float>, IHealable<float>
{
    // These will all be modifiable by the player's active chord modifiers
    [HideInInspector]
    public Vector2 aimingDirection;

    [Header("Abilities")]
    public Ability dashAbility;

    public Ability firstAbility;
    public Ability secondAbility;
    public Ability thirdAbility;
    public Ability fourthAbility;

    private bool coreStatChange = false; // indicates that one of our core stats has changed and a refresh is required.
    [Header("Core")]
    public StatProfileScriptableObject statProfile;
    private int currentHealth;
    public float maxHealthMultiplier = 1;
    private int maxHealth;
    public float baseMoveSpeed = 4;
    public float moveSpeedMultipler = 1;
    public float moveSpeed = 4;
    public float airAccelerationTime = 0.1f;
    public float groundAccelerationTime = 0.05f;
    public float velocityXSmoothing;
    public float gravity = -21f;
    

    [Space(10)]
    [Header("Jump")]
    public float baseMaxJumpHeight = 2.2f;
    public float jumpHeightMultipler = 1;
    public float minJumpHeight = 2.2f / 4;
    public  float maxJumpHeight = 2.2f;
    public int maxNumberofJumps = 2;
    public int numberOfJumpsLeft = 2;
    public float timeToJumpApex = 0.45f;
    public float maxJumpVelocity;
    public float minJumpVelocity;
    private bool jumpAllowed = false;

    [Space(10)]

    [Header("Dash")]
    public int numberOfDashes = 2;
    public int numberOfDashesLeft = 2;
    public float fullDashTime = 0.15f;
    public float dashTimeLeft;
    public float dashSpeedMultiplier = 4;
    public float dashChargeCooldownTime = 2;
    public float dashChargeCooldownTimeLeft;

    [Space(10)]
    [Header("Combat")]
    public float damageMultipler = 1;
    public float knockbackMultiplier = 1;
    public float meleeDamageMultiplier = 1;
    public float rangedDamageMultiplier = 1;

    [Space(10)]
    [Header("Status")]
    public bool playerInvulnerable = false;
    public bool gravityEnabled = true;
    public bool isTeleporting = false;
    public bool isAbleToAttack = true;
    public bool isGrounded = false;
    public bool isRunning = false;
    public bool isDashing = false;
    public bool isDashingEnd = false;

    public GameObject currentInteractableObject;
    public bool isCurrentInteractableObjectLocked;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    private Vector3 oldPosition;
    public Vector2 velocity;

    // Events
    public GameEventScriptableObject playerDeathEvent;


    public void refreshCoreStats()
    {
        maxHealth = statProfile.physicalValue() * 30;
        numberOfDashes = statProfile.mentalBonus / 2;
        dashChargeCooldownTime = 16f - statProfile.mentalBonus;
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    //Set variable values here
    private void Awake()
    {
        aimingDirection.x = 1;
        aimingDirection.y = 0;
        currentHealth = maxHealth;
        minJumpHeight = maxJumpHeight / 4f;
        playerInvulnerable = false;
        playerAlive = true;
        isAbleToAttack = true;
    }

    // Use this for initialization
    void Start()
    {
        refreshCoreStats();
        numberOfDashesLeft = numberOfDashes;

        oldPosition = transform.position;
        velocity = Vector2.zero;
        velocity.x = 0;
        velocity.y = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (coreStatChange)
        {
            refreshCoreStats();
            coreStatChange = false;
        }
    }

    void FixedUpdate()
    {
        ////This block calculates the total time the player should be in the "Dashing" state and counts down until this time is reached to reset our dash state.
        //if (isDashing) 
        //{
        //    dashTimeLeft -= Time.deltaTime;
        //    if (dashTimeLeft <= 0) 
        //    {
        //        dashTimeLeft = 0;
        //        isDashing = false;
        //        isDashingEnd = true;
        //    }
        //}


        //// Dash Cooldown
        //// This dash charge reset code first checkes to see if we reached our cooldown time since last frame. If we have, the timer resets
        //// If we didn't reach our cooldown time last frame, we subtract the time it took to reach our current frame
        //// If we are now at or below our remaining cooldown time, we regain a dash charge. 
        //// On the next frame the game will see that we are at or below 0 cooldown remaining and reset the cooldown.

        //// If we have used a dash charge and it has not recharged yet
        //if (numberOfDashesLeft < numberOfDashes) 
        //{   
        //    // Check to see if the cooldown for the dash charge has passed
        //    if (dashChargeCooldownTimeLeft <= 0) // if it has...
        //    {   
        //        // Set the cooldown back to the maximum cooldown time
        //        dashChargeCooldownTimeLeft = dashChargeCooldownTime;
        //    }


        //    // If we still have time left on our cooldown
        //    dashChargeCooldownTimeLeft -= Time.deltaTime; // reduce the cooldown by time since our last frame
        //    if (dashChargeCooldownTimeLeft <= 0) // if we are now at or below our cooldown time
        //    {
        //        numberOfDashesLeft++; // add a dash charge
        //    }
        //}

        
    }

    public void Damage(float damageTaken)
    {
        if (!playerInvulnerable)
        {
            if (currentHealth - damageTaken > 0)
            {
                currentHealth = (int)(currentHealth - damageTaken);
            }else
            {
                currentHealth = 0;
                playerAlive = false;
                playerDeathEvent.Raise();
            }
        }
    }

    public bool isPlayerAlive()
    {
        return playerAlive;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        if (newMaxHealth > maxHealth) 
        {
            currentHealth += (int)(newMaxHealth - maxHealth);
        }
        maxHealth = (int)newMaxHealth;
       
        
    }

    public void SetJumpHeight(float jumpHeight)
    {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }





    public void Heal(float healAmount)
    {
        throw new System.NotImplementedException();
    }

    public void UseJump()
    {

        isGrounded = false;
        numberOfJumpsLeft -= 1;

    }

    public void ResetJumps()
    {
        numberOfJumpsLeft = maxNumberofJumps;
    }

    public bool JumpAllowed()
    {
        if (numberOfJumpsLeft > 0)
        {
            return true;
        }
        return false;
    }
    
}
