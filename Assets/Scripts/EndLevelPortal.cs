using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPortal : MonoBehaviour , Interactable
{

    public bool isInteractable;
    public bool unlocked;

    // Events
    public GameEventScriptableObject portalEntered;

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
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (playerStats != null)
        {
            if (gameController.gameData.numberOfEnemies > 0)
            {
                unlocked = false;
            }
            else
            {
                unlocked = true;
            }

            if (isInteractable)
            {
                playerStats.currentInteractableObjects.Add(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null) 
        {
            if (isInteractable)
            {
                playerStats.currentInteractableObjects.RemoveAt(playerStats.currentInteractableObjects.IndexOf(this.gameObject)); ;
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
            GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gameController.LoadNextLevel();
        }
    }
}
