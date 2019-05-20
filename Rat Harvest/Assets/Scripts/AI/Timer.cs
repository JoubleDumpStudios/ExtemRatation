using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    Text preRoundText;

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

    // Start is called before the first frame update
    void Start()
    {
        preRoundText.text = "Round Starts In:";

        MinsAndSecondsConverter(ref minsAndSecsHarvestingTime_String, harvestingTime);
        MinsAndSecondsConverter(ref minsAndSecsstartRoundTime_String, startRoundTime);
    }

    void MinsAndSecondsConverter(ref string outPutString, float timeToChange )
    {
        int mins = Mathf.FloorToInt(timeToChange / 60F);
        int secs = Mathf.FloorToInt(timeToChange - mins * 60);
        outPutString = string.Format("{0:00}:{1:00}", mins, secs);
    }

    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;

        if (time >= startRoundTime)
        {
            TurnOnSpawnerAndPlantGrowing();
        }

        MinsAndSecondsConverter(ref minsAndSecsTime_String, time);

        PreroundLogicAndText();


        if (resetTimer)
        {
            time = 0;
            resetTimer = false;
        }
    }

    void PreroundLogicAndText()
    {
        if (preRound)
        {
            if (time <= startRoundTime)
                preRoundText.text = "Round Starts In " + minsAndSecsstartRoundTime_String + "  -> " + minsAndSecsTime_String/*time.ToString("F2")*/;
            else if (time <= startRoundTime + ratsAreAwakeTextTime)
                preRoundText.text = " Rats are awake !!!";
            else
            {
                preRoundText.text = "";
                preRound = false;
                resetTimer = true;
            }
        }
        else if (time <= harvestingTime /*+ startRoundTime + ratsAreAwakeTextTime*/)
        {
            preRoundText.text = "You have " + minsAndSecsHarvestingTime_String + " seconds to harvest how much you can " + minsAndSecsTime_String /*time.ToString("F2")*/;
        }
        else if (time > harvestingTime)
        {
            preRoundText.text = "Round Over";
        }
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
        }
    }
}
