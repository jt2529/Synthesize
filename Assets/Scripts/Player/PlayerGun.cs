using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{

    public GameObject PlayerBullet;
    public PlayerStats stats;

    // Use this for initialization
    void Start()
    {
        stats = gameObject.GetComponentInParent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FireBullet() {
        Vector2 direction = stats.aimingDirection;
        FireBullet(direction);
    }

    public void FireBullet(Vector2 direction)
    {
        GameObject bullet = (GameObject)Instantiate(PlayerBullet);
        bullet.transform.position = transform.position;
        bullet.GetComponent<HarmfulObject>().damage = (int)(bullet.GetComponent<HarmfulObject>().baseDamage * stats.rangedDamageMultiplier);

        BulletPhysics bulletPhysics = bullet.GetComponent<BulletPhysics>();
        bulletPhysics.SetDirection(direction);
        bulletPhysics.playerSpeed = stats.velocity;
        bulletPhysics.weight = bulletPhysics.weight * stats.knockbackMultiplier;
    }
}
