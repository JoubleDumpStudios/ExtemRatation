using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private List<Spawner> spawnerList;

    private bool startRound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateSpawner()
    {
        for (int i = 0; i < spawnerList.Count; i++)
            spawnerList[i].StartRound = true;
    }

    void updatePlant(Spawner spawner)
    {
        int i = 0;


        if (spawner.PatchOfSoil.PlantStandPoints[i].HasCrop)
        {
            //spawner.PatchOfSoil.PlantStandPoints.Plant
        }

    }
}
