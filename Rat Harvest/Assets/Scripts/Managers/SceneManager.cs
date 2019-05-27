using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    [SerializeField] private Texture2D cursorTexture;

    private void Awake()
    {
        instance = this;
    }

    public void loadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void loadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_LevelDesign_Jon");       
        Cursor.SetCursor(null, Vector2.one, CursorMode.Auto);
    }  

    public void loadCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    public void loadControls()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Controls");
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void EnableCursorTexture()
    {
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void DisableCursorTexture()
    {
        Cursor.SetCursor(null, Vector2.one, CursorMode.Auto);
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
