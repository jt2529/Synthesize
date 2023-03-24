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
    public float maxHealthMultiplier;
    private int maxHealth;
    public float baseMoveSpeed;
    public float moveSpeedMultipler;
    public float moveSpeed;

    [Space(10)]
    [Header("Jump")]
    public float baseMaxJumpHeight;
    public float jumpHeightMultipler;
    public float minJumpHeight;
    public  float maxJumpHeight;
    public int numberOfJumps;
    [HideInInspector] public int numberOfJumpsLeft;
    public float timeToJumpApex;

    [Space(10)]

    [Header("Dash")]
    public int numberOfDashes;
    [HideInInspector] public int numberOfDashesLeft;
    public float fullDashTime = 0.15f;
    [HideInInspector] public float dashTimeLeft;
    public float dashSpeedMultiplier;
    public float dashChargeCooldownTime;
    [HideInInspector] public float dashChargeCooldownTimeLeft;

    [Space(10)]
    [Header("Combat")]
    public float damageMultipler;
    public float knockbackMultiplier;
    public float meleeDamageMultiplier;
    public float rangedDamageMultiplier;

    [Space(10)]
    [Header("Status")]
    public bool playerInvulnerable;
    [HideInInspector]
    public bool isTeleporting;
    [HideInInspector]
    public bool isAbleToAttack;
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isRunning;
    [HideInInspector]
    public bool isDashing;
    [HideInInspector]
    public bool isDashingEnd;

    public GameObject currentInteractableObject;
    public bool isCurrentInteractableObjectLocked;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    //private Transform currentTransform;
    private Vector3 oldPosition;
    public Vector2 currentSpeed;

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
        currentSpeed = Vector2.zero;
        currentSpeed.x = 0;
        currentSpeed.y = 0;
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
        currentSpeed.x = Mathf.Abs(transform.position.x - oldPosition.x);
        currentSpeed.y = Mathf.Abs(transform.position.x - oldPosition.x);
        oldPosition = transform.position;
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

    public void SetNumberOfJumps(int jumps)
    {
        numberOfJumps = jumps;
    }

    public void SetJumpHeight(float jumpHeight)
    {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetPlayerVulnerable()
    {
        playerInvulnerable = false;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetNumberOfJumps()
    {
        return numberOfJumps;
    }

    public float GetMaxJumpHeight()
    {
        return maxJumpHeight;
    }

    public float GetMinJumpHeight()
    {
        return minJumpHeight;
    }

    public float GetTimeToJumpApex()
    {
        return timeToJumpApex;
    }

    public Vector2 GetAimingDirection()
    {
        return aimingDirection;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public bool GetPlayerInvulnerable()
    {
        return playerInvulnerable;
    }

    public bool GetPlayerAlive()
    {
        return playerAlive;
    }

    public void hurt(int dmg)
    {
        ChangeHealth(-dmg);
    }

    public void kill()
    {
        ChangeHealth(-this.health);
    }

    public void AddKeyItem(int keyItemNumber)
    {
        keyItems.Add(keyItemNumber);
    }

    public void ClearKeyItems()
    {
        keyItems.Clear();
    }

    public int GetKeyItemsCount()
    {
        return keyItems.Count;
    }

    public void ChangeJumpHeight(float statMultiplier)
    {
        jumpHeightMultipler += statMultiplier;
        SetJumpHeight(baseMaxJumpHeight * jumpHeightMultipler);
    }

    public void ChangeMoveSpeed(float statMultiplier)
    {
        moveSpeedMultipler += statMultiplier;
        SetMoveSpeed(baseMoveSpeed * moveSpeedMultipler);
    }

    public void ChangeMeleeDamage(float statMultiplier)
    {
        meleeDamageMultiplier += statMultiplier;
    }

    public void ChangeRangedDamage(float statMultiplier)
    {
        rangedDamageMultiplier += statMultiplier;
    }

    public void ChangeKnockback(float statMultiplier)
    {
        knockbackMultiplier += statMultiplier;
    }

    public void ChangeJumps(float statMultiplier)
    {
        numberOfJumps = numberOfJumps + 1;
    }

    public void Heal(float healAmount)
    {
        throw new System.NotImplementedException();
    }
}
