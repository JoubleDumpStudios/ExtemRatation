using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused = false; 
    [SerializeField] private GameObject pauseMenuUI;
    UnityStandardAssets.Characters.FirstPerson.MouseLook mouse = new UnityStandardAssets.Characters.FirstPerson.MouseLook();

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
        gameIsPaused = true;
        Time.timeScale = 0f;
        mouse.SetCursorLock(false);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        //Time.timeScale = 1f;
        SceneManager.instance.loadGame();
    }

    public void Controls()
    {
        SceneManager.instance.loadControls();
    }

    public void Menu()
    {
        //Time.timeScale = 1f;
        SceneManager.instance.loadMenu();
    }
}
