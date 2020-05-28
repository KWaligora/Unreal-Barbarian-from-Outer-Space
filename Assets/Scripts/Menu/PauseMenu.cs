using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;

    public GameObject pauseMenuUI;
    public Button backButton;
    public GameObject camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
            if (isPaused)
                Resume();
            else
                Pause();
            }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        backButton.Select();
        isPaused = true;
        Time.timeScale = 0.0f;
        camera.GetComponent<AudioSource>().volume = 0.03f;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        pauseMenuUI.SetActive(false);
        camera.GetComponent<AudioSource>().volume = 0.1f;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
