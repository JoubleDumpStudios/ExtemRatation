using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePrinter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private DontDestroyOnLoadScript dontDesroyScript_;

    [SerializeField]
    private Text scoreText;

    private float score;

    void Start()
    {
        try
        {
            dontDesroyScript_ = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoadScript>();
            score = dontDesroyScript_.Score;
        }
        catch
        {
            score = 1000;
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score : " + score ;
    }
}
