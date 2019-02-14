using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour {

    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            PlayerStats ps = col.GetComponent<PlayerStats>();
            Debug.Log(ps.name + " entered the killbox.");
            ps.hurt(damage);
        }

    }
}
