using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    // These will all be modifiable by the player's active chord modifiers

    public Vector2 aimingDirection;
    

    [SerializeField]
    private int health;

    public float baseMaxJumpHeight;
    public float jumpHeightMultipler;
    public float minJumpHeight;
    public  float maxJumpHeight;

    public int numberOfJumps;
    public int numberOfJumpsLeft;
    public float timeToJumpApex;

    public int numberOfDashes;
    public int numberOfDashesLeft;
    public float fullDashTime;
    public float dashTimeLeft;
    public float dashSpeedMultiplier;
    public float dashChargeCooldownTime;
    public float dashChargeCooldownTimeLeft;

    public float baseMoveSpeed;
    public float moveSpeedMultipler;
    public float moveSpeed;

    public float damageMultipler;

    public float knockbackMultiplier;

    public int baseMaxHealth;
    public float maxHealthMultiplier;
    public int maxHealth;

    public float meleeDamageMultiplier;
    public float rangedDamageMultiplier;

    public bool playerInvulnerable;
    public bool isTeleporting;
    public bool isAbleToAttack;
    public bool isGrounded;
    public bool isRunning;
    public bool isDashing;
    public bool isDashingEnd;

    public GameObject currentInteractableObject;
    public bool isCurrentInteractableObjectLocked;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    private Transform currentTransform;
    private Vector3 oldPosition;
    public Vector2 currentSpeed;

    //Set variable values here
    private void Awake()
    {
        maxHealth = baseMaxHealth;
        aimingDirection.x = 1;
        aimingDirection.y = 0;
        health = maxHealth;
        minJumpHeight = maxJumpHeight / 4f;
        playerInvulnerable = false;
        playerAlive = true;
        isAbleToAttack = true;
        numberOfDashesLeft = numberOfDashes;
    }

    // Use this for initialization
    void Start()
    {
        currentTransform = GetComponent<Transform>();
        oldPosition = currentTransform.position;
        currentSpeed.x = 0;
        currentSpeed.y = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        currentSpeed.x = Mathf.Abs(currentTransform.position.x - oldPosition.x);
        currentSpeed.y = Mathf.Abs(currentTransform.position.x - oldPosition.x);
        oldPosition = currentTransform.position;
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

    public void ChangeMaxHealth(float statMultiplier)
    {
        maxHealthMultiplier += statMultiplier;
        SetMaxHealth(baseMaxHealth * maxHealthMultiplier);
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
}
