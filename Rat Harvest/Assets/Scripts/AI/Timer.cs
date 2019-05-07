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
    float time = 0;

    [SerializeField]
    private float ratsAreAwakeTextTime = 3.0f;


    [SerializeField]
    private float harvestingTime;

    bool resetTimer = false;
    bool preRound = true;
    // Start is called before the first frame update
    void Start()
    {
        preRoundText.text = "Round Starts In:";
    }



    // Update is called once per frame
    void Update()
    {
        
        time += Time.deltaTime;

        if (time >= startRoundTime)
        {
            TurnOnSpawnerAndPlantGrowing();
        }

        if (preRound)
        {
            if (time <= startRoundTime)
                preRoundText.text = "Round Starts In " + startRoundTime + " seconds : " + time.ToString("F2");
            else if (time <= startRoundTime + ratsAreAwakeTextTime)
                preRoundText.text = " Rats are awake !!!";
            else
            {
                preRoundText.text = "";
                preRound = false;
                resetTimer = true;
            }
        }
        else if(time <= harvestingTime /*+ startRoundTime + ratsAreAwakeTextTime*/)
        {
            preRoundText.text = "You have " + harvestingTime + " seconds to harvest how much you can " + time.ToString("F2");
        }else if (time > harvestingTime)
        {
            preRoundText.text = "Round Over";
        }


        if (resetTimer)
        {
            time = 0;
            resetTimer = false;
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
