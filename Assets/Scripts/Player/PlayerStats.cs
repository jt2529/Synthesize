using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    // These will all be modifiable by the player's active chord modifiers

    public Vector2 aimingDirection;
    public int maxHealth = 100;

    [SerializeField]
    private int health;

    private int numberOfJumps;
    private float minJumpHeight;
    public float maxJumpHeight;
    public float timeToJumpApex;
    public float moveSpeed;
    private bool playerInvulnerable;
    public bool isTeleporting;
    public bool isAbleToAttack;
    public bool isGrounded;
    public bool isRunning;

    public GameObject currentInteractableObject;
    public bool isCurrentInteractableObjectLocked;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    public bool facingRight;
    private float damageMultiplier;
    private Transform currentTransform;
    private Vector3 oldPosition;
    public Vector2 currentSpeed;

    //Set variable values here
    private void Awake()
    {
        aimingDirection.x = 1;
        aimingDirection.y = 0;
        health = maxHealth;
        numberOfJumps = 1;
        minJumpHeight = maxJumpHeight / 4f;
        playerInvulnerable = false;
        playerAlive = true;
        isAbleToAttack = true; 
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
    }

    public bool isPlayerAlive()
    {
        return playerAlive;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
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

    public void SetJumpHeight(int jumpHeight)
    {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }

    public void SetMoveSpeed(int newMoveSpeed)
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
}
