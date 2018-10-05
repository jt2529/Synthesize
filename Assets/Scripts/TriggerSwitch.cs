using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : MonoBehaviour {

    public DeployPlatform target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.toggle();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.toggle();
    }
}
