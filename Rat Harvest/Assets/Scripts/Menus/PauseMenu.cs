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
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            
        
        else
        
                Pause();
                
        }
    }

    private void Pause()
    {
        AkSoundEngine.PostEvent("PauseAll", gameObject);
        pauseMenuUI.SetActive(true);
        setObjectsVisibility(false);
        gameIsPaused = true;
        Time.timeScale = 0f;
        mouse.SetCursorLock(false);
        SceneManager.instance.EnableCursorTexture();
    }

    public void Resume()
    {
        AkSoundEngine.PostEvent("ResumeAll", gameObject);
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

    public void SFX_Button_Hover()
    {
        AkSoundEngine.PostEvent("UI_Switch", gameObject);
    }

    public void SFX_Button_Click()
    {
            AkSoundEngine.PostEvent("UI_Click", gameObject);
    }

}
