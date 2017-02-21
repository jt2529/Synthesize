using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulObject : MonoBehaviour {

    public int damage;
    BoxCollider2D objectHitBox;
    BoxCollider2D playerHitBox;
    GameObject player;
    PlayerStats playerStats;
    public bool destructable;

    void Awake() {
        damage = 20;
        objectHitBox = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHitBox = player.GetComponent<BoxCollider2D>();
        playerStats = player.GetComponent<PlayerStats>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (objectHitBox.IsTouching(playerHitBox)) {
            playerStats.ChangeHealth(-damage);
            if (destructable) {
                Object.Destroy(this);
            }
        }
	}
}
