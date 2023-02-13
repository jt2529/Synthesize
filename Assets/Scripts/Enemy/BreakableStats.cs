using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStats : MonoBehaviour
{
    public int maxHealth;
    public int health;

    //Set variable values here
    private void Awake()
    {
        health = maxHealth;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(int changeAmount)
    {
        Debug.Log("Modifying breakable health by " + changeAmount);
        health = health + changeAmount;

        if (health < 1)
        {
            Destroy(gameObject);
        }
    }
}
