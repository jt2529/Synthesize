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
    public float maxJumpHeight = .6f;
    private float timeToJumpApex;
    public float moveSpeed = 2.0f;
    private bool playerInvulnerable;
    public bool playerAlive;
    private float damageMultiplier;

    //Set variable values here
    private void Awake()
    {
        aimingDirection.x = 1;
        aimingDirection.y = 0;
        health = maxHealth;
        numberOfJumps = 1;
        minJumpHeight = maxJumpHeight / 4f;
        timeToJumpApex = .3f;
        playerInvulnerable = false;
        playerAlive = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
}
