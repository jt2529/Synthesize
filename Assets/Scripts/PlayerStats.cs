using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // These will all be modifiable by the player's active chord modifiers

    private int maxHealth;
    private int health;
    private int numberOfJumps;
    private float minJumpHeight;
    private float maxJumpHeight;
    private float timeToJumpApex;
    private float moveSpeed;
    private bool playerDirection;
    private bool playerInvulnerable;
    private bool playerAlive;

    private float damageMultiplier;

    //Set variable values here
    private void Awake() {
        maxHealth = 100;
        health = maxHealth;
        numberOfJumps = 1;
        maxJumpHeight = .6f;
        minJumpHeight = maxJumpHeight / 4f;
        timeToJumpApex = .3f;
        moveSpeed = 2.0f;
        playerDirection = true;
        playerInvulnerable = false;
        playerAlive = true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }

    public void ChangeHealth(int changeAmount) {

        if (health + changeAmount >= maxHealth)
        {
            health = maxHealth;
        }
        else {
            if (changeAmount > 0) {
                health = health + changeAmount;
            }
            else 
            if (!playerInvulnerable) { 
                Debug.Log("Modifying health by " + changeAmount);
                health = health + changeAmount;

                if (health < 1)
                {
                    Destroy(gameObject);
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

    public void SetNumberOfJumps(int jumps) {
        numberOfJumps = jumps;
    }

    public void SetJumpHeight(int jumpHeight) {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }

    public void SetMoveSpeed(int newMoveSpeed) {
        moveSpeed = newMoveSpeed;
    }

    public void SetPlayerDirection(bool direction)
    {
        playerDirection = direction;
    }

    public void SetPlayerVulnerable() {
        playerInvulnerable = false;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public int GetHealth() {
        return health;
    }

    public int GetNumberOfJumps() {
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

    public float GetMoveSpeed() {
        return moveSpeed;
    }

    public bool GetPlayerDirection() {
        return playerDirection;
    }

    public bool GetPlayerInvulnerable() {
        return playerInvulnerable;
    }
}
