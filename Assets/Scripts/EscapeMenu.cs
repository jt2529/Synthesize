using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{

    public GameObject[] menuObjects;
    public bool isOpen;

    // Use this for initialization
    void Start()
    {
        isOpen = false;
        hideMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //controls the pausing of the scene
    public void MenuControl()
    {
        if (isOpen)
        {
            Time.timeScale = 1;
            hideMenu();
            isOpen = false;
        }
        else
        {
            Time.timeScale = 0;
            showMenu();
            isOpen = true;
        }
    }

    public void Save() 
    {
        GameController.controllerInstance.Save();
        
    }

    public void Load()
    {
        GameController.controllerInstance.LoadScene(GameController.controllerInstance.gameData.activeSceneName, true);
    }

    //shows objects with ShowOnPause tag
    public void showMenu()
    {
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public void hideMenu()
    {
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        GameController.controllerInstance.LoadScene(GameController.controllerInstance.gameData.activeSceneName, false);
    }
}