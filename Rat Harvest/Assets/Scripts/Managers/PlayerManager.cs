using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Variables
    [SerializeField] private int playerHealth = 100;

    // Texts
    [SerializeField] private Text scoreText;

    // Icons
    [SerializeField] private Image aimIcon;
    [SerializeField] private Image plantIcon;
    [SerializeField] private Image HarvestIcon;

    // Player's score
    private int playerScore = 0;
    public int PlayerScore { get { return this.playerScore; } set { this.playerScore = value; } }


    private void Start()
    {
        DisablePlantIcon();
        DisableHarvestIcon();
    }

    public void updateScore()
    {
        scoreText.text = "Score: " + playerScore;
    }

    public void EnableAimIcon()
    {
        aimIcon.enabled = true;
    }

    public void DisableAimIcon()
    {
        aimIcon.enabled = false;
    }

    public void EnablePlantIcon()
    {
        plantIcon.enabled = true;
    }

    public void DisablePlantIcon()
    {
        plantIcon.enabled = false;
    }

    public void EnableHarvestIcon()
    {
        HarvestIcon.enabled = true;
    }

    public void DisableHarvestIcon()
    {
        HarvestIcon.enabled = false;
    }
}
