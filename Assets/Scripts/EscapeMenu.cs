using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{

    GameObject[] menuObjects;
    bool isOpen;

    // Use this for initialization
    void Start()
    {
        isOpen = false;
        menuObjects = GameObject.FindGameObjectsWithTag("MenuItem");
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
            hideMenu();
            isOpen = false;
        }
        else
        {
            showMenu();
            isOpen = true;
        }
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
        Scene scene =SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}