using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public EnemyStats stats;
    public float fullDisplayTime;
    private Vector3 localScale;
    private float xScale;
    private float displayTime;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        xScale = localScale.x;
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayTime > 0) 
        {
            displayTime -= Time.deltaTime;
            if (displayTime <= 0) 
            {
                HideHealthBar();
            }
        }
   
    }

    public void ShowHealthBar()
    {
        displayTime = fullDisplayTime;
        spriteRenderer.enabled = true;
        localScale.x = xScale * (float)stats.health / (float)stats.maxHealth;
        transform.localScale = localScale;
    }

    public void HideHealthBar()
    {
        displayTime = 0;
        spriteRenderer.enabled = false;
    }
}
