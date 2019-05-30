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
    [SerializeField] private Image fillingAmmoIcon;
    [SerializeField] private Image fillingHarvestIcon;


    // Player's score
    private int playerScore;
    public int PlayerScore { get { return this.playerScore; } }

    // Player's Harvest
    private int playerHarvest = 0;
    public int PlayerHarvest { get { return this.playerHarvest; } }

    [SerializeField]
    private DontDestroyOnLoadScript dontDesroyScript_;

    private void Start()
    {
        bagText.text = playerHarvest + " / " + bagCapacity;
        scoreText.text = playerScore + " / " + GameManager.instance.PointsToWin;

        DisablePlantIcon();
        DisableHarvestIcon();

        try { dontDesroyScript_ = GameObject.FindGameObjectWithTag("DontDestroy").GetComponent<DontDestroyOnLoadScript>(); } catch { }
        UpdateEndGameScores();
    }

    public void updateScore()
    {
        if(playerHarvest != 0)
            AkSoundEngine.PostEvent("HUD_FoodBox", gameObject);

        playerScore += playerHarvest;
        scoreText.text = playerScore + " / " + GameManager.instance.PointsToWin;

        playerHarvest = 0;
        bagText.text = playerHarvest + " / " + bagCapacity;


        UpdateEndGameScores();
    }

    private void UpdateEndGameScores()
    {
        try {
            dontDesroyScript_.Score = playerScore;
            dontDesroyScript_.UpdateScore(playerScore);
        } catch { }
    }

    public void updateBag(int harvest)
    {

        if (playerHarvest + harvest > bagCapacity)
            playerHarvest += bagCapacity - playerHarvest;
        else
            playerHarvest += harvest;

        bagText.text = playerHarvest + " / " + bagCapacity;
    }

    public void EnablePlantIcon()
    {
        plantIcon.enabled = true;
    }

    public void DisablePlantIcon()
    {
        if (plantIcon.enabled)
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

    public void DisableFillingAmmoIcon()
    {
        if (fillingAmmoIcon.enabled)
            fillingAmmoIcon.enabled = false;
    }

    public void EnableFillingAmmoIcon()
    {
        fillingAmmoIcon.enabled = true;
    }

    public void EnableFillingHarvestIcon()
    {
        fillingHarvestIcon.enabled = true;
    }

    public void DisableFillingHarvestIcon()
    {
        fillingHarvestIcon.enabled = false;
    }
}
