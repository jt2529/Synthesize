using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public int maxHealth;
    public float moveSpeed;
    public float baseMovementDampener, movementDampener, smoothTime;
    public int health;
    public int rewards;
    public int baseCurrencyReward;
    public int currencyReward;
    public float jumpHeight;
    public float smoothing;
    public float timeToJumpApex;
    public float weight;
    public float fireRate;
    public float aggroDistance;
    public HealthBar healthBar;
    public Vector2 force;
    public float stunTimer, attackCoolDown;
    public bool isStunned, canAttack;
    public Animator anim;
    public List<PatternSequencerMap> movementPatterns;
    public List<PatternSequencerMap> attackPatterns;
    public int currentMovementPatternIndex, currentAttackPatternIndex, currentMovementSequencerIndex, currentAttackSequencerIndex;
    public BeatTimer beatTimer;

    //Set variable values here
    private void Awake()
    {
        health = maxHealth;
    }

    // Use this for initialization
    void Start() {
        stunTimer = 0;
        isStunned = false;
        attackCoolDown = 0;
        currentMovementPatternIndex = 0;
        currentAttackPatternIndex = 0;
        beatTimer = FindObjectOfType<BeatTimer>();
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
        if (attackCoolDown > 0) 
        {
            canAttack = false;
            attackCoolDown -= Time.deltaTime;
            if (attackCoolDown <= 0)
            {
                attackCoolDown = 0;
            }
        }

        int nextBeat;
        if (movementPatterns.Count > 0 && attackPatterns.Count > 0)
        { 
            if (beatTimer.currentMeasure == 1)
            {
                currentMovementPatternIndex = 0;
                currentAttackPatternIndex = 0;
            }
            else
            {
                if ( (currentMovementPatternIndex < movementPatterns.Count - 1) && beatTimer.currentMeasure >= movementPatterns[currentMovementPatternIndex + 1].startMeasure) 
                {
                    currentMovementPatternIndex++;
                }

                if ( (currentAttackPatternIndex < attackPatterns.Count - 1) && beatTimer.currentMeasure >= movementPatterns[currentAttackPatternIndex + 1].startMeasure)
                {
                    currentAttackPatternIndex++;
                }
            }

            if (beatTimer.currentBeat == 1) 
            {
                currentMovementSequencerIndex = 0;
                currentAttackSequencerIndex = 0;
            }

            if (beatTimer.currentBeat == beatTimer.totalBeatsInPattern)
            {
                if (currentMovementPatternIndex != movementPatterns.Count - 1)
                {
                    if (movementPatterns[currentMovementPatternIndex + 1].startMeasure == beatTimer.currentMeasure + 1)
                    {
                        nextBeat = movementPatterns[currentMovementPatternIndex + 1].sequencer[0];
                    }
                    else
                    {
                        nextBeat = movementPatterns[currentMovementPatternIndex].sequencer[0];
                    }
                }
                else 
                {
                    nextBeat = movementPatterns[currentMovementPatternIndex].sequencer[0];
                }
            }
            else 
            {
                nextBeat = movementPatterns[currentMovementPatternIndex].sequencer[currentMovementSequencerIndex];
            }

            if (beatTimer.currentBeat == nextBeat - 1 || ( beatTimer.currentBeat == beatTimer.totalBeatsInPattern && nextBeat == 1))
            {
                smoothTime = beatTimer.GetNextBeat() - beatTimer.song.time;
                movementDampener = Mathf.SmoothDamp(movementDampener, 1, ref smoothing, smoothTime);
            }
            else if (beatTimer.currentBeat == movementPatterns[currentMovementPatternIndex].sequencer[currentMovementSequencerIndex])
            {
                currentMovementSequencerIndex++;
                if (currentMovementSequencerIndex == movementPatterns[currentMovementPatternIndex].sequencer.Count) 
                {
                    currentMovementSequencerIndex = 0;
                }
                movementDampener = Mathf.SmoothDamp(movementDampener, baseMovementDampener, ref smoothing, smoothTime);
            }

            if (beatTimer.currentBeat == attackPatterns[currentAttackPatternIndex].sequencer[currentAttackSequencerIndex])
            {
                if (attackCoolDown == 0)
                {
                    canAttack = true;
                }    
                
                currentAttackSequencerIndex++;
                if (currentAttackSequencerIndex == attackPatterns[currentAttackPatternIndex].sequencer.Count)
                {
                    currentAttackSequencerIndex = 0;
                }
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
        if (stunTime > stunTimer) //If enemy is not stunned for longer than this attack already, then set stunTimer
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
            Destroy(gameObject);
        }
    }

    public void DropRewards() 
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (rewards > 0) 
        {
            for (int i = 0; i < rewards; i++)
            {
                gameController.DropLoot(transform.position);
            }
        }

        if (currencyReward > 0) 
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().ModifyCurrency(currencyReward);
        }

        gameController.EnemyKilled();
    }
}
