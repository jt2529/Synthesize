using System.Collections;
using System.Collections.Generic;
using Synthesize.Weapons;
using Synthesize.CoreSystem;
using UnityEngine;

public class WeaponPickup : MonoBehaviour , Interactable
{
    [SerializeField] private SpriteRenderer weaponIcon;
    [SerializeField] private WeaponDataSO weaponData;
    private WeaponDataSO swappedWeaponData;
    public WeaponDataSO GetContext() => weaponData;

    public void SetContext(WeaponDataSO context)
    {
        weaponData = context;

        weaponIcon.sprite = weaponData.Icon;
    }
    // Start is called before the first frame update
    void Awake()
    {
        weaponIcon = GetComponent<SpriteRenderer>();
        weaponIcon.sprite = weaponData.Icon;
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
            playerStats.currentInteractableObjects.Add(this.gameObject);
        } 
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

        WeaponInventory weaponInventory = playerObject.GetComponentInChildren<WeaponInventory>();
        PlayerStats playerStats = playerObject.GetComponentInChildren<PlayerStats>();
        weaponInventory.TrySetWeapon(weaponData, playerStats.currentSelectedWeapon, out swappedWeaponData);
        if (swappedWeaponData != null)
        {
            weaponIcon.sprite = swappedWeaponData.Icon;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
