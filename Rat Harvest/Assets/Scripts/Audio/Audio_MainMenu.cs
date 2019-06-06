using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("MX_MainMenu", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
