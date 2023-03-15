using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerAbilitiesController : MonoBehaviour {

    public Dictionary<int, KeyCode> noteMap;
    public PlayerGun primary;
    public PlayerWeapon secondary;
    public PlayerStats stats;
    public Keytar keytar;
    public int[] lastThreeNotes;
    public EscapeMenu menu;
    public Camera mainCamera;

    private Animator state;
    private MovementPhysics controller;

    // Use this for initialization
    void Start () {

        controller = GetComponent<MovementPhysics>();
        state = GetComponent<Animator>();
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
    }
	
	// Update is called once per frame
	void Update () {
        /*if (!stats.isPlayerAlive())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.MenuControl();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if(controller.collisions.below)
            {
                attack();
            } else
            {
                jumpAttack();
            }
        }

        for (int i = 0; i < noteMap.Count; i++)
        {
            if (Input.GetKeyDown(noteMap[i]))
            {
                keytar.Play(i);
            }
            else if (Input.GetKeyUp(noteMap[i]))
            {
                keytar.Release(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            lastThreeNotes = keytar.GetLastPlayed();
        }*/
    }

    void OnInteract(InputValue value) 
    {
        TriggerInteract();
    }

    void OnFire(InputValue value) 
    {   
        if (Mathf.Abs(stats.aimingDirection.x) < 0.1 && Mathf.Abs(stats.aimingDirection.y) < 0.1)
        {
            if (stats.facingRight)
            {
                stats.aimingDirection.x = 1;
            }
            else
            {
                stats.aimingDirection.x = -1;
            }
            stats.aimingDirection.y = 0;
            primary.FireBullet();
            stats.aimingDirection.x = 0;
            stats.aimingDirection.y = 0;
        }
        else 
        {
            primary.FireBullet();
        }
    }

    void OnFireMouse(InputValue value) 
    {
        stats.aimingDirection = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 
        primary.FireBullet();
    }

    void OnSecondaryFire(InputValue value) 
    {
        if (stats.isAbleToAttack) 
        { 
            secondary.MeleeAttack(); 
        }

        stats.isAbleToAttack = false;
        if (stats.isGrounded)
        {
            state.SetBool("Attack", true);
        }
        else 
        {
            state.SetBool("JumpAttack", true);
        }
    }

    public void EndAttack()
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
    }

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
