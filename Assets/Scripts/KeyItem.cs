using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour , Interactable
{
    public GameObject otherPortal;
    public bool isInteractable;
    public int keyItemNumber;
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
        if (!playerStats.isCurrentInteractableObjectLocked) 
        {
            playerStats.currentInteractableObject = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null) 
        {
            if (playerStats.currentInteractableObject == this.gameObject)
            {
                playerStats.currentInteractableObject = null;
            }
        }
    }

    public void Interact(GameObject playerObject) 
    {
        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        playerStats.keyItems.Add(keyItemNumber);
        Destroy(gameObject);
    }
}
