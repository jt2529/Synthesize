using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Synthesize.CoreSystem;
using Synthesize.Weapons;

public class PlayerAbilitiesInputHandler : MonoBehaviour {

    public Dictionary<int, KeyCode> noteMap;
   // public PlayerGun primary;
    //public PlayerWeapon secondary;
    public PlayerStats stats;
    public Keytar keytar;
    public int[] lastThreeNotes;
    public EscapeMenu menu;
    public Camera mainCamera;
    public PlayerInput input;
    private MovementPhysics physics;
    private Core core;
    private Animator state;
    private WeaponInventory weaponInventory;
    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    public bool[] AttackInputs { get; private set; }

    public event Action OnAttackAction;

    // Use this for initialization
    void Start () {
        core = GetComponentInChildren<Core>();
        physics = core.GetCoreComponent<MovementPhysics>();
        state = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
        weaponInventory = core.GetCoreComponent<WeaponInventory>();
        primaryWeapon = GetComponentInChildren<Weapon>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        noteMap = new Dictionary<int, KeyCode>() {
            { 0, KeyCode.U },
            { 1, KeyCode.I },
            { 2, KeyCode.O },
            { 3, KeyCode.P },
            { 4, KeyCode.J },
            { 5, KeyCode.K },
            { 6, KeyCode.L },
            { 7, KeyCode.Semicolon }

        
        };
        

        
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
    }
	
	// Update is called once per frame
	void Update () {

    }

    void OnInteract(InputValue value) 
    {
        TriggerInteract();
    }

    void OnFire(InputValue value) 
    {   
        if (Mathf.Abs(stats.aimingDirection.x) < 0.1 && Mathf.Abs(stats.aimingDirection.y) < 0.1)
        {
            if (physics.facingDirection > 0)
            {
                stats.aimingDirection.x = 1;
            }
            else
            {
                stats.aimingDirection.x = -1;
            }
            stats.aimingDirection.y = 0;
            primaryWeapon.CurrentInput = true;
            //primary.FireBullet();
            AttackInputs[(int)CombatInputs.primary] = true;
            stats.aimingDirection.x = 0;
            stats.aimingDirection.y = 0;
        }
        else 
        {
            //primary.FireBullet();
            AttackInputs[(int)CombatInputs.primary] = true;
            primaryWeapon.CurrentInput = true;
        }
    }

    void OnFireMouse(InputValue value)
    {
        stats.aimingDirection = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        if (stats.PrimaryAttackState.CanTransitionToAttackState())
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }
        
        //primary.FireBullet();
    }

    void OnSecondaryFire(InputValue value) 
    {
        if (stats.isAbleToAttack) 
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        /*stats.isAbleToAttack = false;
        if (stats.isGrounded)
        {
            state.SetBool("Attack", true);
        }
        else 
        {
            state.SetBool("JumpAttack", true);
        }*/
    }

    private void OnPause(InputValue value)
    {
        
        menu.MenuControl();
        if (menu.isOpen)
        {
            input.SwitchCurrentActionMap("UI");
        }
        else 
        {
            input.SwitchCurrentActionMap("Player");
        }
    }

    /*public void EndAttack()
    {
        stats.isAbleToAttack = true;
        state.SetBool("Attack", false);
        secondary.EndMeleeAttack();
    }

    public void EndJumpAttack()
    {
        state.SetBool("JumpAttack", false);
        stats.isAbleToAttack = true;
        secondary.EndMeleeAttack();
    }*/

    public void UseAttackInput(int i) => AttackInputs[i] = false;
    void TriggerInteract() 
    {
        if (stats.currentInteractableObjects.Count > 0) 
        {
            if (stats.currentInteractableObjectLocked != null)
            {
                stats.currentInteractableObjectLocked.Interact(this.gameObject);
            }
            else 
            {
                Interactable[] interactableObjects = stats.currentInteractableObjects[stats.currentInteractableObjects.Count - 1].GetComponents<Interactable>();
                foreach (Interactable interactableObject in interactableObjects)
                {
                    if (stats.currentInteractableObjectLocked == null || stats.currentInteractableObjectLocked == interactableObject)
                    {
                        interactableObject.Interact(this.gameObject);
                    }
                }
            }
        }
    }
    
}

public enum CombatInputs
{
    primary,
    secondary
}
