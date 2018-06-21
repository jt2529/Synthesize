using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulObject : MonoBehaviour {

    public int damage;
    BoxCollider2D objectHitBox;
    public bool destructable;

    void Awake() {
        objectHitBox = GetComponent<BoxCollider2D>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
     
	}

    public void tryDestroy()
    {
        if (destructable) {
            damage = 0;
            Destroy(gameObject);
        }
    }
}
