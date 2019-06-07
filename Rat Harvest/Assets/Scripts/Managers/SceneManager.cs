using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private Texture2D cursorTexture;

    private void Awake()
    {
        instance = this;
    }


    public void loadMenu()
    {
        AkSoundEngine.PostEvent("MX_StopLooseScreen", gameObject);
        AkSoundEngine.PostEvent("MX_StopWinScreen", gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void loadGame()
    {
        AkSoundEngine.PostEvent("MX_StopMainMenu", gameObject);
        AkSoundEngine.PostEvent("MX_StopLooseScreen", gameObject);
        AkSoundEngine.PostEvent("MX_StopWinScreen", gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);       
        Cursor.SetCursor(null, Vector2.one, CursorMode.Auto);
    }  

    public void loadCredits()
    {
        AkSoundEngine.PostEvent("MX_StopLooseScreen", gameObject);
        AkSoundEngine.PostEvent("MX_StopWinScreen", gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    public void loadControls()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Controls");
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void loadYouWin()
    {
        AkSoundEngine.PostEvent("MX_WinScreen", gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouWin");
        Cursor.SetCursor(cursorTexture, Vector2.one, CursorMode.Auto);
    }

    public void loadYouLose()
    {
        AkSoundEngine.PostEvent("MX_LooseScreen", gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene("YouLose");
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
