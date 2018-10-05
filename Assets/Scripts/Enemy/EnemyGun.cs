using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public GameObject EnemyBullet;
    public GameObject aimAtObject;
    // Use this for initialization
    void Start () {
        InvokeRepeating("FireBullet", 1f, 2f);
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void FireBullet() {

        if (aimAtObject != null) {

            GameObject bullet = (GameObject)Instantiate(EnemyBullet);

            bullet.transform.position = transform.position;
            Vector2 direction = aimAtObject.transform.position - transform.position;
            bullet.GetComponent<BulletPhysics>().SetDirection(direction);
        }
    }
}


