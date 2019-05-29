using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePrinter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DontDestroyOnLoadScript dontDesroyScript_;

    void Start()
    {
        try { dontDesroyScript_ = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoadScript>(); } catch { }
    }

    // Update is called once per frame
    void Update()
    {
        try { Debug.Log(dontDesroyScript_.Score); } catch { }
    }
}
