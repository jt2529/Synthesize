using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable<float>, IHealable<float>
{
    // These will all be modifiable by the player's active chord modifiers
    [HideInInspector]
    public Vector2 aimingDirection;

    private bool coreStatChange = false; // indicates that one of our core stats has changed and a refresh is required.
    [Header("Core")]
    public StatProfileScriptableObject statProfile;
    private int health;
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
    public int numberOfJumps = 2;
    public int numberOfJumpsLeft = 2;
    public float timeToJumpApex = 0.45f;
    public float maxJumpVelocity;
    public float minJumpVelocity;

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
    public bool playerInvulnerable;
    public bool isTeleporting;
    public bool isAbleToAttack;
    public bool isGrounded;
    public bool isRunning = false;
    public bool isDashing = false;
    public bool isDashingEnd = false;

    public GameObject currentInteractableObject;
    public bool isCurrentInteractableObjectLocked;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    //private Transform currentTransform;
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
        health = maxHealth;
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
        //oldPosition = transform.position;
        if (isDashing) 
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0) 
            {
                dashTimeLeft = 0;
                isDashing = false;
                isDashingEnd = true;
            }
        }

        if (numberOfDashesLeft < numberOfDashes) 
        {
            if (dashChargeCooldownTimeLeft <= 0)
            {
                dashChargeCooldownTimeLeft = dashChargeCooldownTime;
            }

            dashChargeCooldownTimeLeft -= Time.deltaTime;
            if (dashChargeCooldownTimeLeft <= 0) 
            {
                numberOfDashesLeft++;
            }
        }
    }

    public void Damage(float damageTaken)
    {
        if (!playerInvulnerable)
        {
            if (health - damageTaken >= 0)
            {
                health = (int)(health - damageTaken);
            }else
            {
                health = 0;
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
            health += (int)(newMaxHealth - maxHealth);
        }
        maxHealth = (int)newMaxHealth;
       
        
    }

    public void ChangeHealth(int changeAmount)
    {

        if (health + changeAmount >= maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            if (changeAmount > 0)
            {
                health = health + changeAmount;
            }
            else
            if (!playerInvulnerable)
            {
                Debug.Log("Modifying health by " + changeAmount);
                health = health + changeAmount;

                if (health < 1)
                {
                    playerAlive = false;
                    Debug.Log("Player Died");
                }
                if (changeAmount < 0)
                {
                    playerInvulnerable = true;

                    // Set this back equal to false in 3 seconds.
                    Invoke("SetPlayerVulnerable", 3);
                }
            }

        }


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

    
}
