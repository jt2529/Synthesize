using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public int maxHealth;
    public float moveSpeed;
    public int health;
    public int rewards;
    public int baseCurrencyReward;
    public int currencyReward;
    public float jumpHeight;
    public float timeToJumpApex;
    public float weight;
    public float fireRate;
    public float aggroDistance;
    public HealthBar healthBar;
    public Vector2 force;
    public float stunTimer;
    public bool isStunned;
    public bool isAlive;
    public Animator anim;

    //Set variable values here
    private void Awake()
    {
        health = maxHealth;
    }

    // Use this for initialization
    void Start() {
        stunTimer = 0;
        isStunned = false;
        isAlive = true;
    }

    // Update is called once per frame
    void Update() {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) 
            {
                anim.SetBool("Stunned", false);
                stunTimer = 0;
                isStunned = false;
            }
        }
    }

    public void SetCurrencyReward(float currencyRewardMultiplier) 
    {
        currencyReward = (int)(currencyRewardMultiplier * (float)baseCurrencyReward);
    }

    public void ReceiveAttack(int damage, Vector2 knockbackForce, float stunTime) 
    {
        ChangeHealth(damage);   //Damage the enemy
        force += knockbackForce; //Add a knockback force to the enemy (referenced by Enemy Physics Controller)
        if (stunTime > stunTimer && isAlive) //If enemy is not stunned for longer than this attack already, then set stunTimer
        {
            stunTimer = stunTime;
            isStunned = true;
            anim.SetBool("Stunned", true);
        }
    }

    public void ChangeHealth(int changeAmount)
    {
        Debug.Log("Modifying enemy health by " + changeAmount);
        health = health + changeAmount;
        healthBar.ShowHealthBar();

        if (health < 1)
        {
            DropRewards();
            isAlive = false;
            Destroy(gameObject);
        }
    }

    public void DropRewards() 
    {
        if (rewards > 0) 
        {
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            for (int i = 0; i < rewards; i++)
            {
                gameController.DropLoot(transform.position);
            }
        }

        if (currencyReward > 0) 
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ModifyCurrency(currencyReward);
        }
        
    }
}
