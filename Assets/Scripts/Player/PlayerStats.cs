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
    public Jump jumpAbility;

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
    public float dropDuration = 0.1f;
    
    public float wallSlideSpeedDampener;
    

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

    // A better inventory system will need to be built but idk what else to call this for now.
    [Space(10)]
    [Header("Inventory?")]
    public List<GameObject> currentInteractableObjects;
    public List<int> keyItems;
    public Interactable currentInteractableObjectLocked;
    public bool isCurrentInteractableObjectLocked;
    

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
    public bool isWallSliding = false;
    public bool isWallJumping = false;
    [SerializeField] private bool isDropping = false;
    public bool horizontalInput = false;
    private int playerFacingDirection = 1;



    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    private Vector3 oldPosition;
    public Vector2 velocity;

    // Events
    public GameEventScriptableObject playerDeathEvent;
    public GameEventScriptableObject playerNearInteractableObjectEvent;
    public GameEventScriptableObject playerLeftRangeOfInteractableObjectEvent;

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

        currentInteractableObjects = new List<GameObject>();
        
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

        if(velocity.x > 0)
        {
            playerFacingDirection = 1;
        }
        else if(velocity.x < 0)
        {
            playerFacingDirection = -1;
        }
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

    public bool isFalling()
    {
        if(velocity.y < 0)
        {
            return true;
        }
        return false;
    }

    public bool IsDropping()
    {
        return isDropping;
    }

    public void SetIsDropping(bool dropping)
    {
        isDropping = dropping;
    }

    public int GetKeyItemsCount()
    {
        return keyItems.Count;
    }

    public void AddNearbyInteractableObject(GameObject interactable)
    {
        currentInteractableObjects.Add(interactable);
        playerNearInteractableObjectEvent.Raise();
    }

    public void RemoveNearbyInteractableObject(GameObject interactable)
    {
        currentInteractableObjects.RemoveAt(currentInteractableObjects.IndexOf(interactable));

        if(currentInteractableObjects.Count == 0)
        {
            playerLeftRangeOfInteractableObjectEvent.Raise();
        }
    }

    public bool IsNearInteractableObject()
    {
        if(currentInteractableObjects.Count > 0)
        {
            return true;
        }
        
        return false;
        
    }

    public void AddKeyItem(int keyItemNumber)
    {
        keyItems.Add(keyItemNumber);
    }
    
    public bool IsWallSliding()
    {
        return isWallSliding;
    }

    public void SetWallSlide(bool isSliding)
    {
        isWallSliding = isSliding;
    }

    public void SetWallJumping(bool walling)
    {
        isWallJumping = walling;
    }

    public bool IsWallJumping()
    {
        return isWallJumping;
    }

    public void HorizontalInputReceived()
    {
        horizontalInput = true;
    }

    public int PlayerFacingDirection()
    {
        return playerFacingDirection;
    }
}
