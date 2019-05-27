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

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "00:00"/* "Round Starts In:"*/;

        time = startRoundTime + 1;
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
        if(!endGame)
            time -= Time.deltaTime;

        if (time <= 0 && preRound)
        {
            TurnOnSpawnerAndPlantGrowing();
            time = harvestingTime +1;
            preRound = false;
            AkSoundEngine.PostEvent("Rat_StartTimer", gameObject);
        }
        else if (time <= 0 && !preRound)
        {
            //endGame;
            endGame = true;
            time = 0;
            GameManager.instance.EndGame();
        }
        if (time == 12.8f && preRound)
        {
            Debug.Log("Time is 12.8");
            AkSoundEngine.PostEvent("MX_Windup01", gameObject);
        }

        timeText.text = MinsAndSecondsConverter(time);

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
