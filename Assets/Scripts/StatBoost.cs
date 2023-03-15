using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoost : MonoBehaviour , Interactable
{
    public enum StatType { JumpHeight, MoveSpeed, Health, MeleeDamage, RangedDamage, Knockback, Jumps };

    public StatType statType;
    public float statBoost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null) 
        {
            playerStats.currentInteractableObjects.Add(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null) 
        {
             playerStats.currentInteractableObjects.RemoveAt(playerStats.currentInteractableObjects.IndexOf(this.gameObject));
        }
    }

    public void Interact(GameObject playerObject) 
    {
        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        switch(statType) 
        {
            case StatType.JumpHeight:
                playerStats.ChangeJumpHeight(statBoost);
                break;
            case StatType.MoveSpeed:
                playerStats.ChangeMoveSpeed(statBoost);
                break;
            case StatType.Health:
                playerStats.ChangeMaxHealth(statBoost);
                break;
            case StatType.MeleeDamage:
                playerStats.ChangeMeleeDamage(statBoost);
                break;
            case StatType.RangedDamage:
                playerStats.ChangeRangedDamage(statBoost);
                break;
            case StatType.Knockback:
                playerStats.ChangeKnockback(statBoost);
                break;
            case StatType.Jumps:
                playerStats.ChangeJumps(statBoost);
                break;
        }
        Destroy(gameObject);
    }
}
