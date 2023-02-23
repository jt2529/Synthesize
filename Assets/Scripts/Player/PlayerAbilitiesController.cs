﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerAbilitiesController : MonoBehaviour {

    public Dictionary<int, KeyCode> noteMap;
    public PlayerGun gun;
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
        triggerInteract();
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
            gun.FireBullet();
            stats.aimingDirection.x = 0;
            stats.aimingDirection.y = 0;
        }
        else 
        {
            gun.FireBullet();
        }
    }

    void OnFireMouse(InputValue value) 
    {
        new Vector2 mousePosition = 
        stats.aimingDirection = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()); 
        gun.FireBullet();
    }

    void attack()
    {
        state.SetBool("Attack", true);
    }

    public void endAttack()
    {
        state.SetBool("Attack", false);
    }

    void jumpAttack()
    {
        state.SetBool("JumpAttack", true);
    }

    public void endJumpAttack()
    {
        state.SetBool("JumpAttack", false);
    }

    void triggerInteract() 
    {
        if (stats.currentInteractableObject != null) 
        {
            Interactable[] interactableObjects = stats.currentInteractableObject.GetComponents<Interactable>();
            foreach (Interactable interactableObject in interactableObjects)
            {
                interactableObject.Interact(this.gameObject);
            }
        }
    }
    
}
