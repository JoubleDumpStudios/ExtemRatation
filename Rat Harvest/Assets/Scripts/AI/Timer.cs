using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Text timeText;

    [SerializeField]
    private List<Spawner> spawnerList;

    [SerializeField]
    private int startRoundTime;
    private string minsAndSecsstartRoundTime_String;


    private float fstartRoundTime;
    float time = 0;

    [SerializeField]
    private float ratsAreAwakeTextTime = 3.0f;


    [SerializeField]
    private float harvestingTime;

    private string minsAndSecsTime_String;
    private string minsAndSecsHarvestingTime_String;

    bool resetTimer = false;
    bool preRound = true;
    bool endGame = false;

    private 

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "00:00"/* "Round Starts In:"*/;

        time = startRoundTime + 1;

        fstartRoundTime = time;

        PreRoundEvents();

        RoundEvents();
    }

    string MinsAndSecondsConverter(float timeToChange)
    {
        int mins = Mathf.FloorToInt(timeToChange / 60F);
        int secs = Mathf.FloorToInt(timeToChange - mins * 60);
        string outPutString = string.Format("{0:00}:{1:00}", mins, secs);

        return outPutString;
    }

    // Update is called once per frame
    void Update()
    {
        if (!endGame)
        {
            time -= Time.deltaTime;
        }

        if (time <= 0 && preRound)
        {
            TurnOnSpawnerAndPlantGrowing();
            time = harvestingTime +1;
            preRound = false;
        }
        else if (time <= 0 && !preRound)
        {
            //endGame;
            endGame = true;
            time = 0;
            GameManager.instance.EndGame();
        }

        timeText.text = MinsAndSecondsConverter(time);

    }


    //You Have to create one method for each event and Invoke them all from this methods
    void PreRoundEvents()
    {
        
        Invoke("MX_WindUp", 1.27f);
        //Invoke("Name of the method you want to play", YourTime)
        Invoke("Rat_Scream", 13);
    }

    void RoundEvents()
    {
        Invoke("MX_CombatStart", fstartRoundTime + 0);

        //Invoke("Name of the method you want to play", fstartRoundTime + YourTime), 
        //this will call you method in YourTime seconds after the start of the round.

        Invoke("MX_CombatEnd", fstartRoundTime + 297);

        Invoke("HUD_ClockOut", fstartRoundTime + 291);

        Invoke("MX_Outro", fstartRoundTime + 297);

    





    }

    void MX_WindUp()
    {

        AkSoundEngine.PostEvent("MX_Windup01", gameObject);
    }
    void Rat_Scream()
    {
        AkSoundEngine.PostEvent("Rat_StartTimer", gameObject);
    }

    void MX_CombatStart()
    {
        AkSoundEngine.PostEvent("MX_CombatLoop01", gameObject);
    }

    void MX_CombatEnd()
    {
        AkSoundEngine.PostEvent("MX_CombatStop", gameObject);
    }

    void MX_Outro()
    {
        AkSoundEngine.PostEvent("MX_Outro", gameObject);
    }

    void HUD_ClockOut()
    {
        AkSoundEngine.PostEvent("HUD_CountDown", gameObject);
    }



    void TurnOnSpawnerAndPlantGrowing()
    {
        for (int i = 0; i < spawnerList.Count; i++)
        {
            spawnerList[i].StartRound = true;
            UpdatePlant(spawnerList[i]);
        }
    }

    void UpdatePlant(Spawner spawner)
    {
        List<PlantPoint> plantPoints = spawner.PatchOfSoil.PlantPoints;

        for (int i = 0; i < plantPoints.Count; i++)
        {
            PlantPoint plantPoint = plantPoints[i];
            if (plantPoint.HasCrop)
            {
                plantPoint.Plant.GetComponent<Plant_Behaviour>().StartRound = true;
            }
            plantPoint.RoundStarted = true;
        }
    }
}
