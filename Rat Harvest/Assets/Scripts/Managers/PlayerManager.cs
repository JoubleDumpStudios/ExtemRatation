using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Variables
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private Text scoreText;

    // Player's score
    private int playerScore = 0;
    public int PlayerScore { get { return this.playerScore; } set { this.playerScore = value; } }

    public void updateScore()
    {
        scoreText.text = "Score: " + playerScore;
    }
}
