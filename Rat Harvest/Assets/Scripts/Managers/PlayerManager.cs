using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    // Variables
    [SerializeField] private int playerHealth = 100;
    [SerializeField] private int bagCapacity;
    public int BagCapacity { get { return this.bagCapacity; } }

    [SerializeField] private int maxAmmo;
    public int MaxAmmo { get { return this.maxAmmo; } }

    // Texts
    [SerializeField] private Text scoreText;
    [SerializeField] private Text bagText;

    // Icons
    [SerializeField] private Image plantIcon;
    [SerializeField] private Image HarvestIcon;


    // Player's score
    private int playerScore = 0;
    public int PlayerScore { get { return this.playerScore; } }

    // Player's Harvest
    private int playerHarvest = 0;
    public int PlayerHarvest { get { return this.playerHarvest; } }


    private void Start()
    {
        DisablePlantIcon();
        DisableHarvestIcon();
    }

    public void updateScore()
    {
        playerScore += playerHarvest;
        scoreText.text = playerScore.ToString();

        playerHarvest = 0;
        bagText.text = playerHarvest.ToString();
    }

    public void updateBag(int harvest)
    {

        if (playerHarvest + harvest > bagCapacity)
            playerHarvest += bagCapacity - playerHarvest;
        else
            playerHarvest += harvest;

        bagText.text = playerHarvest.ToString();
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
