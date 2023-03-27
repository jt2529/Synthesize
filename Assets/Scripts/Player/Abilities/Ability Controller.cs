using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{

    public Ability rightTriggerAbility;
    public Ability leftTriggerAbility;
    public Ability AButtonAbility;
    public PlayerStats playerStats;

    private bool rightTriggerEnabled = true;
    private bool leftTriggerEnabled = true;
    private bool AButtonEnabled = true;

    private GameEventListener movementAbilityEndListener;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rightTriggerEnabled)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetAxis("R Trigger") == 1)
            {
                rightTriggerAbility.beginAbility();
            }

            if (Input.GetButtonDown("Reload"))
            {
                rightTriggerAbility.reload();
            }
        }

        if (leftTriggerEnabled)
        {
            if (Input.GetButtonDown("Fire2") || Input.GetAxis("L Trigger") != 0)
            {
                leftTriggerAbility.beginAbility();
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (AButtonEnabled)
            {
                AButtonAbility.beginAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            //playerStats.Damage(damageValue);
            Debug.Log("You pressed J. Congrats!");
        }
    }

    public bool PrimaryEnabled
    {
        get
        {
            return rightTriggerEnabled;
        }

        set
        {
            rightTriggerEnabled = value;
        }
    }

    public bool SecondaryEnabled
    {
        get
        {
            return SecondaryEnabled;
        }

        set
        {
            SecondaryEnabled = value;
        }
    }

    public bool MovementAbilityEnabled
    {
        get
        {
            return AButtonEnabled;
        }

        set
        {
            AButtonEnabled = value;
        }
    }

}
