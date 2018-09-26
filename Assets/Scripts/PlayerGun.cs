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
        Debug.Log(direction.y);
        FireBullet(direction);
    }

    public void FireBullet(Vector2 direction)
    {
        GameObject bullet = (GameObject)Instantiate(PlayerBullet);
        bullet.transform.position = transform.position;
        bullet.GetComponent<BulletPhysics>().SetDirection(direction);
    }
}
