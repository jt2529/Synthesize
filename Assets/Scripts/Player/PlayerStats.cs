using System.Collections;
using System.Collections.Generic;
using Synthesize.Combat.Damage;
using Synthesize.CoreSystem;
using Synthesize.Weapons;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public static PlayerStats playerStats;
    // These will all be modifiable by the player's active chord modifiers
    [HideInInspector]
    public Vector2 aimingDirection;

    private bool coreStatChange = false; // indicates that one of our core stats has changed and a refresh is required.
    [Header("Core")]
    public Core Core;
    public StatProfileScriptableObject statProfile;
    private int health;
    public float maxHealthMultiplier;
    public int baseMaxHealth;
    private int maxHealth;
    public float baseMoveSpeed;
    public float moveSpeedMultipler;
    public float moveSpeed;
    public float standColliderHeight;
    public float crouchColliderHeight;
    public float crouchSpeedDampener;
    public float accelerationTimeAirborne = .1f;
    public float accelerationTimeGrounded = .05f;
    public float decelerationTimeAirborne = 1f;
    public float decelerationTimeGrounded = .5f;
    public Vector2 currentVelocityInput;

    [Space(10)]
    [Header("Jump")]
    public float baseMaxJumpHeight;
    public float jumpHeightMultipler;
    public float minJumpHeight;
    public float maxJumpHeight;
    public float gravity;
    public float originalGravity;
    public float maxJumpVelocity;
    public float minJumpVelocity;
    public int numberOfJumps;
    [HideInInspector] public int numberOfJumpsLeft;
    public float timeToJumpApex;
    public float wallJumpPower;
    public float wallJumpHeight;
    public float wallSlideSpeedDampener;

    [Space(10)]

    [Header("Dash")]
    public int numberOfDashes;
    public float fullDashTime = 0.15f;
    public float dashSpeedMultiplier;
    public float dashChargeCooldownTime;

    [Space(10)]
    [Header("Combat")]
    public float damageMultipler;
    public float knockbackMultiplier;
    public float meleeDamageMultiplier;
    public float rangedDamageMultiplier;
    [SerializeField]
    private Weapon primaryWeapon;
    [SerializeField]
    private Weapon secondaryWeapon;
    public int currentSelectedWeapon { get; set; }
    public float stunTime;

    //private Weapon secondaryWeapon;

    [Space(10)]
    [Header("Status")]
    public bool playerInvulnerable;
    public bool isTeleporting;
    public bool isAbleToAttack;
    public bool isGrounded;
    //public bool isRunning;
    public bool isDashing;
    public bool isDashingEnd;
    public bool isDropping;
    public bool isWallSliding;
    public bool isAttacking;

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerStunState PlayerStunState { get; private set; }


    public BoxCollider2D collider;
    public List<GameObject> currentInteractableObjects;
    public Interactable currentInteractableObjectLocked;
    public GameObject objectTouchingLeft;
    public GameObject objectTouchingRight;
    public GameObject objectTouchingAbove;
    public GameObject objectTouchingBelow;

    public Animator Anim;
    public PlayerAbilitiesInputHandler abilitiesInputHandler;
    public PlayerPhysicsInputHandler physicsInputHandler;

    public int currency;
    public List<int> keyItems;

    [SerializeField]
    public bool playerAlive;
    private Vector3 oldPosition;
    public Vector2 currentSpeed;
    private Vector2 workspace;

    // Events
    public GameEventScriptableObject playerDeathEvent;
    public PlayerAbilitiesInputHandler playerAbilitiesInputHandler;
    public PlayerPhysicsInputHandler playerPhysicsInputHandler;

    public void refreshCoreStats()
    {
        maxHealth = statProfile.physicalValue() * 30;
        numberOfDashes = statProfile.mentalBonus / 2;
        dashChargeCooldownTime = 16f - statProfile.mentalBonus;
    }

    //Set variable values here
    private void Awake()
    { 
        Core = GetComponentInChildren<Core>();
        maxHealth = baseMaxHealth;
        aimingDirection.x = 1;
        aimingDirection.y = 0;
        health = maxHealth;
        minJumpHeight = maxJumpHeight / 4f;
        currentVelocityInput = new Vector2(0, 0);
        playerInvulnerable = false;
        playerAlive = true;
        isAbleToAttack = true;
        isAttacking = false;

        updatePlayerPhysics();
        originalGravity = gravity;

        currentSelectedWeapon = 0;

        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);

        playerAbilitiesInputHandler = GetComponent<PlayerAbilitiesInputHandler>();
        playerPhysicsInputHandler = GetComponent<PlayerPhysicsInputHandler>();

        collider = GetComponent<BoxCollider2D>();
        SetColliderHeight(standColliderHeight);
        Anim = GetComponent<Animator>();
        physicsInputHandler = GetComponent<PlayerPhysicsInputHandler>();
        abilitiesInputHandler = GetComponent<PlayerAbilitiesInputHandler>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, "move");
        JumpState = new PlayerJumpState(this, StateMachine, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, "inAir");
        LandState = new PlayerLandState(this, StateMachine, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, "inAir");
        DashState = new PlayerDashState(this, StateMachine, "dash");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, "attack", primaryWeapon, CombatInputs.primary);
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, "attack", secondaryWeapon, CombatInputs.secondary);
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, "crouchMove");
        PlayerStunState = new PlayerStunState(this, StateMachine, "stun");
    }

    // Use this for initialization
    void Start()
    {
        //refreshCoreStats();
        StateMachine.Initialize(IdleState);
        oldPosition = transform.position;
        currentSpeed.x = 0;
        currentSpeed.y = 0;
        DashState.ResetCanDash();
    }

    // Update is called once per frame
    void Update()
    {
        if (coreStatChange)
        {
            refreshCoreStats();
            coreStatChange = false;
        }
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        updatePlayerPhysics();
        StateMachine.CurrentState.PhysicsUpdate();
        currentSpeed.x = transform.position.x - oldPosition.x;
        currentSpeed.y = transform.position.y - oldPosition.y;
        oldPosition = transform.position;

        //Dash charge logic needs to be kept here as it is on a timer and refills regardless of which state player is in
        if (DashState.numberOfDashesLeft < numberOfDashes) 
        {
            if (DashState.dashRecoveryStartTime != 0 && (Time.time - DashState.dashRecoveryStartTime) > dashChargeCooldownTime) 
            {
                DashState.DashChargeRecovery();

                //Reset the dashStartTime
                if (DashState.numberOfDashesLeft < numberOfDashes)
                {
                    DashState.dashRecoveryStartTime = Time.time;
                }
                else 
                {
                    DashState.dashRecoveryStartTime = 0;
                }
            }
        }
    }

    public void Damage(DamageData damageData)
    {
        if (!playerInvulnerable)
        {
            if (health - damageData.Amount >= 0)
            {
                health = (int)(health - damageData.Amount);
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

    public void ModifyCurrency(int changeAmount) 
    {
        currency += changeAmount;
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

    public void SetHealth(int newHealth) 
    {
        health = newHealth;
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

    public void ChangeMaxHealth(float amount)
    {
        maxHealthMultiplier += amount;
        SetMaxHealth(maxHealthMultiplier * baseMaxHealth);
    }

    public void SetColliderHeight(float height)
    {
        Vector2 center = collider.offset;
        workspace.Set(collider.size.x, height);

        center.y += (height - collider.size.y) / 2;

        collider.size = workspace;
        collider.offset = center;
    }

    void updatePlayerPhysics()
    {
        gravity = -(2 * GetMaxJumpHeight()) / Mathf.Pow(GetTimeToJumpApex(), 2);
        maxJumpVelocity = Mathf.Abs(gravity) * GetTimeToJumpApex();
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * GetMinJumpHeight());
    }

    public void SetVelocityX(float x)
    {
         currentVelocityInput.x = x;
    }
    public void SetVelocityY(float y)
    {
        currentVelocityInput.y = y;
    }

    public void SetVelocity(Vector2 vector)
    {
        currentVelocityInput.x = vector.x;
        currentVelocityInput.y = vector.y;
    }

}
