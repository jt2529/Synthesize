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

    private float damageMultiplier;

    //Set variable values here
    private void Awake() {
        maxHealth = 100;
        health = maxHealth;
        numberOfJumps = 1;
        maxJumpHeight = .8f;
        minJumpHeight = maxJumpHeight / 4f;
        timeToJumpApex = .3f;
        moveSpeed = 2.5f;
        playerDirection = true;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void setMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
    }

    public void changeHealth(int changeAmount) {
        health = health + changeAmount;
    }

    public void setNumberOfJumps(int jumps) {
        numberOfJumps = jumps;
    }

    public void setJumpHeight(int jumpHeight) {
        maxJumpHeight = jumpHeight;
        minJumpHeight = jumpHeight / 4f;
    }

    public void setMoveSpeed(int newMoveSpeed) {
        moveSpeed = newMoveSpeed;
    }

    public void setPlayerDirection(bool direction)
    {
        playerDirection = direction;
    }

    public int getMaxHealth() {
        return maxHealth;
    }

    public int getHealth() {
        return health;
    }

    public int getNumberOfJumps() {
        return numberOfJumps;
    }

    public float getMaxJumpHeight()
    {
        return maxJumpHeight;
    }

    public float getMinJumpHeight()
    {
        return minJumpHeight;
    }

    public float getTimeToJumpApex()
    {
        return timeToJumpApex;
    }

    public float getMoveSpeed() {
        return moveSpeed;
    }

    public bool getPlayerDirection() {
        return playerDirection;
    }
}
