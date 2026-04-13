using System;
using System.Collections;
using System.Collections.Generic;
using Synthesize.CoreSystem;
using Synthesize.Weapons;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }
    public PlayerStunState PlayerStunState { get; private set; }

    [SerializeField]
    private PlayerStats stats;

    public PlayerPhysicsInputHandler physicsInputHandler { get; private set; }
    #endregion

    #region Components
    public Core Core { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerAbilitiesInputHandler abilitiesInputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Transform DashDirectionIndicator { get; private set; }
    public BoxCollider2D MovementCollider { get; private set; }
    public Stats Stats { get; private set; }
    
    #endregion

    #region Other Variables         

    private Vector2 workspace;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        Core = GetComponentInChildren<Core>();
        physicsInputHandler = GetComponent<PlayerPhysicsInputHandler>();

        primaryWeapon = transform.Find("PrimaryWeapon").GetComponent<Weapon>();
        secondaryWeapon = transform.Find("SecondaryWeapon").GetComponent<Weapon>();
        
        primaryWeapon.SetCore(Core);
        secondaryWeapon.SetCore(Core);

        Stats = Core.GetCoreComponent<Stats>();
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this.stats, StateMachine, "idle");
        MoveState = new PlayerMoveState(this.stats, StateMachine, "move");
        JumpState = new PlayerJumpState(this.stats, StateMachine, "inAir");
        InAirState = new PlayerInAirState(this.stats, StateMachine, "inAir");
        LandState = new PlayerLandState(this.stats, StateMachine, "land");
        WallSlideState = new PlayerWallSlideState(this.stats, StateMachine, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this.stats, StateMachine, "inAir");
        DashState = new PlayerDashState(this.stats, StateMachine, "inAir");
        PrimaryAttackState = new PlayerAttackState(this.stats, StateMachine, "attack", primaryWeapon, CombatInputs.primary);
        SecondaryAttackState = new PlayerAttackState(this.stats, StateMachine, "attack", secondaryWeapon, CombatInputs.secondary);
        PlayerStunState = new PlayerStunState(this.stats, StateMachine, "stun");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("DashDirectionIndicator");
        MovementCollider = GetComponent<BoxCollider2D>();

        Stats.Poise.OnCurrentValueZero += HandlePoiseCurrentValueZero;
        
        StateMachine.Initialize(IdleState);
    }

    private void HandlePoiseCurrentValueZero()
    {
        StateMachine.ChangeState(PlayerStunState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void OnDestroy()
    {
        Stats.Poise.OnCurrentValueZero -= HandlePoiseCurrentValueZero;
    }

    #endregion

    #region Other Functions

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }   

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

   
    #endregion
}
