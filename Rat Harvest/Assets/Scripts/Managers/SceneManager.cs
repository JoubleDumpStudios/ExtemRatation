using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void loadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void loadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_LevelDesign_Jon");
    }  

    public void loadCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    public void loadControls()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Controls");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
