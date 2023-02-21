using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour , Interactable
{
    public GameObject otherPortal;
    public bool isInteractable;
    public int keyItemsToUnlock;
    public bool unlocked;
    // Start is called before the first frame update
    void Start()
    {
        unlocked = false;
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
            if (playerStats.GetKeyItemsCount() < keyItemsToUnlock)
            {
                unlocked = false;
            }
            else
            {
                unlocked = true;
            }

            if (isInteractable)
            {
                if (!playerStats.isCurrentInteractableObjectLocked)
                {
                    playerStats.currentInteractableObject = this.gameObject;
                }
            }
            else
            {
                if (!playerStats.isTeleporting)
                {
                    collision.transform.position = otherPortal.transform.position;
                }

                if (playerStats.isTeleporting != true)
                {
                    playerStats.isTeleporting = true;
                }
                else
                {
                    playerStats.isTeleporting = false;
                }
            }
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null) 
        {
            if (isInteractable && playerStats.currentInteractableObject == this.gameObject)
            {
                playerStats.currentInteractableObject = null;
            }

            else 
            {
                if (!playerStats.isTeleporting)
                {
                    playerStats.isTeleporting = false;
                }
            }
        }
    }

    public void Interact(GameObject playerObject) 
    {
        if (unlocked) 
        {
            playerObject.transform.position = otherPortal.transform.position;
        }
    }
}
