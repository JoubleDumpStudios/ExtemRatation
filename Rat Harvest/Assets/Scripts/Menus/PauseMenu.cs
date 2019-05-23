using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private bool gameIsPaused = false;
    [SerializeField] private List<GameObject> objsToDisable;
    [SerializeField] private GameObject pauseMenuUI;
    UnityStandardAssets.Characters.FirstPerson.MouseLook mouse = new UnityStandardAssets.Characters.FirstPerson.MouseLook();

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        setObjectsVisibility(true);
        gameIsPaused = false;
    }

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
        setObjectsVisibility(false);
        gameIsPaused = true;
        Time.timeScale = 0f;
        mouse.SetCursorLock(false);
        SceneManager.instance.EnableCursorTexture();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        setObjectsVisibility(true);
        gameIsPaused = false;
        Time.timeScale = 1f;
        mouse.SetCursorLock(true);
        SceneManager.instance.DisableCursorTexture();
    }

    public void Restart()
    {
        pauseMenuUI.SetActive(false);
        setObjectsVisibility(true);
        gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.instance.loadGame();
    }

    public void Controls()
    {
        SceneManager.instance.loadControls();
    }

    public void Menu()
    {
        pauseMenuUI.SetActive(false);
        setObjectsVisibility(true);
        gameIsPaused = false;
        Time.timeScale = 1f;
        SceneManager.instance.loadMenu();
    }

    private void setObjectsVisibility(bool b)
    {
        for (int i = 0; i < objsToDisable.Count; i++)
            objsToDisable[i].SetActive(b);
    }
}
