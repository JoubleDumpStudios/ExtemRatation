using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    UnityStandardAssets.Characters.FirstPerson.MouseLook mouse = new UnityStandardAssets.Characters.FirstPerson.MouseLook();
    
    // Start is called before the first frame update
    void Start()
    {
        mouse.SetCursorLock(false);
        SceneManager.instance.EnableCursorTexture();
    }
}
