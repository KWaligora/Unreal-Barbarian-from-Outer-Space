using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public Button startButton;
    public Button backButton;

    public GameObject menu;
    public GameObject controlMenu;

    void Start()
    {
        startButton.Select();    
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Control()
    {
        menu.active = false;
        controlMenu.active = true;
        backButton.Select();
    }

    public void Back()
    {
        controlMenu.active = false;
        menu.active = true;
        startButton.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

 
}
