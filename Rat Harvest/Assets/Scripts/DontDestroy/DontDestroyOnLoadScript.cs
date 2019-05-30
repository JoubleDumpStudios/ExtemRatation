using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    private int score;
    public int Score { get { return this.score;  } set { this.score = value; } }

    private int maxScore;
    public int MaxScore { get { return this.maxScore; } set { this.maxScore = value; } }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int newScore)
    {
        score = newScore;
        if(score > maxScore) { maxScore = score; }
    }
}
