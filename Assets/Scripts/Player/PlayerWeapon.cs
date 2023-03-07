using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    public Collider2D collider;
    public List<Collider2D> enemyCollisions;
    public int damage;
    private bool isAttacking;
    private List<string> enemyCollisionTracker;
    public PlayerStats stats;
    public float knockbackPower;
    public float stunTime;
    private Vector2 knockbackVector;
    private Vector2 directionalKnockbackVector;

    [SerializeField]
    private ContactFilter2D contactFilter;

    // Use this for initialization
    void Start()
    {
        enemyCollisionTracker = new List<string>();
        stats = gameObject.GetComponentInParent<PlayerStats>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
        contactFilter.useLayerMask = true;
        collider.enabled = false;
        knockbackVector = new Vector2(1, 0) * knockbackPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking) 
        {
            int overlapCount = Physics2D.OverlapCollider(collider, contactFilter, enemyCollisions);
            if (overlapCount <= 1)
            {
                foreach (Collider2D enemyCollision in enemyCollisions)
                {
                    if (!enemyCollisionTracker.Contains(enemyCollision.name)) 
                    {
                        directionalKnockbackVector = knockbackVector * stats.knockbackMultiplier;
                        if (!stats.facingRight)
                        {
                            directionalKnockbackVector = directionalKnockbackVector * -1;
                        }

                        int hitDamage = (int)(damage * stats.meleeDamageMultiplier);
                        enemyCollision.GetComponent<EnemyStats>().Attack(hitDamage, directionalKnockbackVector, stunTime);
                        enemyCollisionTracker.Add(enemyCollision.name);
                    }
                    
                }
            }
        }
    }
    public void MeleeAttack()
    {
        isAttacking = true;
        collider.enabled = true;
    }

    public void EndMeleeAttack() 
    {
        isAttacking = false;
        collider.enabled = false;
        enemyCollisionTracker = new List<string>();
    }
}
