using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilitiesController : MonoBehaviour {

    public Dictionary<int, KeyCode> noteMap;
    public PlayerGun gun;
    public PlayerStats stats;
    public Keytar keytar;
    public int[] lastThreeNotes;
    public EscapeMenu menu;

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
        if (!stats.isPlayerAlive())
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
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            gun.FireBullet();
        }
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
    
}
