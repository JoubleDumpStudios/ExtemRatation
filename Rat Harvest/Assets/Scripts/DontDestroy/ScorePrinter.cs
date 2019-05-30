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

    [SerializeField]
    private Text maxScoreText;

    private float score;
    private float maxScore;

    [SerializeField]
    private bool mainMenuScene;

    void Start()
    {
            try { dontDesroyScript_ = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoadScript>();
                score = dontDesroyScript_.Score;
                maxScore = dontDesroyScript_.MaxScore;
        } catch { score = 1000; maxScore = 4000; }

        updateScoreBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void updateScoreBoard()
    {
        if (!mainMenuScene)
            scoreText.text = "Score : " + score;
        else 
        {
            if (maxScore > 0)
                maxScoreText.text = "Max Score : " + maxScore;
            else maxScoreText.text = "";
        }
    }
}
