﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPooler objectPooler;

    [SerializeField] private Soil patchOfSoil;
    [SerializeField] private GameObject objectToSpawn;
   
    // List to store the rats that spawn 
    private List<GameObject> rats = new List<GameObject>();

    //GameObject to store the specific rat that has been spawned
    private GameObject rat;

    // Script of the rat chosen
    private Rat_Movement_Logic ratScript;

    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler.instance;
        spawnRat();
        spawnRat();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            spawnRat();
    }

    private bool hasDestinations()
    {
        int i = 0;
        while (i < patchOfSoil.PlantStandPoints.Count)
        {
            int j = 0;
            while (j < patchOfSoil.PlantStandPoints[i].PlantEatingPoints.Count)
            {
                if (!patchOfSoil.PlantStandPoints[i].PlantEatingPoints[j].HasRat)
                    return true;

                j++;
            }
            i++;
        }

        return false;
    }

    // spawns a rat
    private void spawnRat()
    {
        if (hasDestinations())
        {
            objectPooler.spawnFromPool(objectToSpawn.name, transform.position, transform.rotation, out rat);
            setRatDestinations();
            ratScript.chooseNewTarget();
            rats.Add(rat);
        }
    }

    // Sets the destinations for each rat created
    private void setRatDestinations()
    {
        ratScript = rat.GetComponent<Rat_Movement_Logic>();

        for (int i = 0; i < patchOfSoil.PlantStandPoints.Count; i++)
            ratScript.setDestinations(patchOfSoil.PlantStandPoints[i].PlantEatingPoints);
    }
}
