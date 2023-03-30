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
    public Ability jumpAbility;

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
    public float minDelayBeforeNextJump = 0.02f;
    private bool jumpAllowed = false;

    [Space(10)]

    [Header("Dash")]
    public int numberOfDashes = 2;
    public int numberOfDashesLeft = 2;
    public float fullDashTime = 0.15f;
    public float dashTimeRemaining;
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
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
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


    public void setPlayerAirborne()
    {
        isGrounded = false;
    }

    public void setPlayerGrounded()
    {
        isGrounded = true;
    }


    public void Heal(float healAmount)
    {
        throw new System.NotImplementedException();
    }

    public void setPlayerIsDashing()
    {
        isDashing = true;
    }

    public void setPlayerDashEnding()
    {
        isDashingEnd = true;
    }

    public void setPlayerNotDashing()
    {
        isDashing = false;
    }

}
