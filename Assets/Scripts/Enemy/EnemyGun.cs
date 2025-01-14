using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {

    public GameObject EnemyBullet;
    public GameObject aimAtObject;
    public EnemyStats stats;
    public float cooldown;
    // Use this for initialization
    void Start () {
        InvokeRepeating("FireBullet", 1f, stats.fireRate);
    }
	
	// Update is called once per frame
	void Update () {
        if (stats.canAttack) 
        {
            FireBullet();
        }
    }

    public void FireBullet() {
        if (aimAtObject == null)
        {
            aimAtObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (aimAtObject != null && !stats.isStunned) {

            GameObject bullet = (GameObject)Instantiate(EnemyBullet);

            bullet.transform.position = transform.position;
            Vector2 direction = aimAtObject.transform.position - transform.position;
            bullet.GetComponent<BulletPhysics>().SetDirection(direction);
        }
        stats.attackCoolDown = cooldown;
    }
}


