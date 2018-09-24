using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUIController : MonoBehaviour {

    public PlayerStats playerStats;
    public Image hpBar;
    public Text hpText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateHealthBar();        
	}

    void updateHealthBar()
    {
        hpBar.fillAmount = playerStats.health / playerStats.maxHealth;
        hpText.text = Mathf.Round((playerStats.health / playerStats.maxHealth) * 100).ToString();
    }
}
