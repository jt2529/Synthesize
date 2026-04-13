using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Synthesize.CoreSystem;

public class HealthBar : CoreComponent
{
    public SpriteRenderer spriteRenderer;
    public float fullDisplayTime;
    private Vector3 localScale;
    private float xScale;
    private float displayTime;
    private Stats stats;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        xScale = localScale.x;
        spriteRenderer.enabled = false;
        stats = core.GetCoreComponent<Stats>();
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
        localScale.x = xScale * (float)stats.Health.CurrentValue / (float)stats.Health.MaxValue;
        transform.localScale = localScale;
    }

    public void HideHealthBar()
    {
        displayTime = 0;
        spriteRenderer.enabled = false;
    }
}
