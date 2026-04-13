using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Synthesize.CoreSystem;

public class EnemyStats : MonoBehaviour {

    public int maxHealth;
    public float moveSpeed;
    public float baseMovementDampener, movementDampener, smoothTime;
    public int rewards;
    public int baseCurrencyReward;
    public int currencyReward;
    public float jumpHeight;
    public float smoothing;
    public float timeToJumpApex;
    public float weight;
    public float fireRate;
    public float aggroDistance;
    public Vector2 force;
    public float attackCoolDown;
    public bool canAttack, isAlive;
    public Animator anim;
    public List<PatternSequencerMap> movementPatterns;
    public List<PatternSequencerMap> attackPatterns;
    public int currentMovementPatternIndex, currentAttackPatternIndex, currentMovementSequencerIndex, currentAttackSequencerIndex;
    public BeatTimer beatTimer;
    public Core core;
    public Stats stats;

    //Set variable values here
    private void Awake()
    {
        core = GetComponentInChildren<Core>();
    }

    // Use this for initialization
    void Start() {
        stats = core.GetCoreComponent<Stats>();
        stats.Health.MaxValue = maxHealth;
        stats.Health.Init();
        attackCoolDown = 0;
        currentMovementPatternIndex = 0;
        currentAttackPatternIndex = 0;
        beatTimer = FindObjectOfType<BeatTimer>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update() {
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
