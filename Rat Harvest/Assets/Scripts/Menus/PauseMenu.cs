using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;


    private void Update()
    {
        CheckInput(); 
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Restart()
    {
        SceneManager.instance.loadGame();
    }

    public void Controls()
    {
        SceneManager.instance.loadControls();
    }

    public void Menu()
    {
        SceneManager.instance.loadMenu();
    }
}
