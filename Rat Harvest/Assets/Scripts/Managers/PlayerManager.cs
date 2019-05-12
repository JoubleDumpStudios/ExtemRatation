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

    public void EnablePlantIcon()
    {
        plantIcon.enabled = true;
    }

    public void DisablePlantIcon()
    {
        if(plantIcon.enabled)
            plantIcon.enabled = false;
    }

    public void EnableHarvestIcon()
    {
        HarvestIcon.enabled = true;
    }

    public void DisableHarvestIcon()
    {
        if(HarvestIcon.enabled)
            HarvestIcon.enabled = false;
    }
}
