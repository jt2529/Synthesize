using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilitiesController : MonoBehaviour {

    public Dictionary<int, KeyCode> noteMap;
    public PlayerGun gun;
    public PlayerStats stats;
    public Keytar keytar;
    public int[] lastThreeNotes;

    // Use this for initialization
    void Start () {
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
        if (stats.playerAlive != true)
        {
            return;
        }

        for (int i = 0; i < noteMap.Count; i++)
        {
            if (Input.GetKeyDown(noteMap[i])) {
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
    
}
