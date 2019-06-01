using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Impact_Metal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Impact_Metal", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
