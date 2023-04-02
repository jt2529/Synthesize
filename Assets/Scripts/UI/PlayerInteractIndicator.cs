using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractIndicator : MonoBehaviour
{
    private bool showIndicator = false;
    private PlayerStats player;
    private SpriteRenderer indicator;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerStats>();
        indicator = GetComponent<SpriteRenderer>();

        indicator.enabled = showIndicator;
    }

    public void ShowIndicator()
    {
        indicator.enabled = true;
    }

    public void HideIndicator()
    {
        indicator.enabled = false;
    }
}
