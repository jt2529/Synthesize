using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBoxController : MonoBehaviour, Interactable {

    private MovementPhysics physics;
    private Vector2 velocity;
    public bool isInteractable;
    public bool isBeingMoved;
    public float pickupOffset;

    public float gravity;

    // Use this for initialization
    void Start () {
        physics = GetComponent<MovementPhysics>();
        isBeingMoved = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //velocity.x = 0;
        if (!isBeingMoved) 
        {
            velocity.y += gravity * Time.deltaTime;
            physics.Move(velocity);
        }
        

    }

    public void Push(Vector2 velocity)
    {
        if (physics.collisions.above || physics.collisions.below)
        {
            velocity.y = 0;
        }

        //if (physics.collisions.left || physics.collisions.right)
        //{
        //    velocity.x = 0;
        //}
   
        physics.Move(velocity);
    }

    public void Interact(GameObject playerObject)
    {

        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        isBeingMoved = !isBeingMoved;
        if (isBeingMoved)
        {
            playerStats.isCurrentInteractableObjectLocked = true;
            Vector3 translation = new Vector3(playerObject.transform.position.x - transform.position.x, playerObject.transform.position.y - transform.position.y + pickupOffset, 0);
            transform.Translate(translation); 
            transform.SetParent(playerObject.transform);
           
        }
        else 
        {
            playerStats.isCurrentInteractableObjectLocked = false;
            transform.SetParent(null);
            velocity.y = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (isInteractable && playerStats != null)
        {
            playerStats.currentInteractableObject = this.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (isInteractable && playerStats != null && playerStats.currentInteractableObject == this.gameObject)
        {
            playerStats.currentInteractableObject = null;
            //isBeingMoved = false;
        }
    }
}
