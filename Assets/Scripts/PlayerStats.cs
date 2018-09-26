using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // These will all be modifiable by the player's active chord modifiers

    public Vector2 aimingDirection;
    public int maxHealth;
    public int health;
    public int numberOfJumps;
    public float minJumpHeight;
    public float maxJumpHeight;
    public float timeToJumpApex;
    public float moveSpeed;
    public float invulnerabilityTime;
    public bool playerInvulnerable;
    public bool playerAlive;
    public float damageMultiplier;

    //Set variable values here
    private void Awake() {

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ChangeHealth(int changeAmount) {

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
            if (!playerInvulnerable) { 
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
                    Invoke("SetPlayerVulnerable", invulnerabilityTime);
                }
            }
            
        }
    }

    public void SetJumpHeight(int jumpHeight) {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }

    public void SetPlayerVulnerable() {
        playerInvulnerable = false;
    }
}
