using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour , Interactable
{
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
        playerStats.currentInteractableObjects.Add(this.gameObject);
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
        playerStats.AddKeyItem(keyItemNumber);
        Destroy(gameObject);
    }
}
