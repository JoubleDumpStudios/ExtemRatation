using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPooler objectPooler;

    [SerializeField] private Soil patchOfSoil;
    public Soil PatchOfSoil { get { return this.patchOfSoil; } }

    [SerializeField] private GameObject objectToSpawn;
   
    // List to store the rats that spawn 
    private List<GameObject> rats = new List<GameObject>();

    //GameObject to store the specific rat that has been spawned
    private GameObject rat;

    // Script of the rat chosen
    private Rat_Movement_Logic ratScript;

    private bool activeSpawner;
    public bool ActiveSpawner { set { this.activeSpawner = value; } }

    float time = 0;

    [SerializeField]
    private float spawningTime = 0;

    [SerializeField]
    private bool startRound;
    public bool StartRound { set { this.startRound = value; } }

    private void Awake()
    {
        objectPooler = ObjectPooler.instance;
        patchOfSoil.Spawner = gameObject.GetComponent<Spawner>();
    }

    private void Update()
    {
        if (startRound && activeSpawner)
        {
            time += Time.deltaTime;
            if (time >= spawningTime)
            {
                spawnRat();
                time = 0;
            }
        }
        //if (Input.GetKeyDown(KeyCode.L))
        //    spawnRat();
    }

    private bool hasDestinations()
    {
        int i = 0;
        while (i < patchOfSoil.PlantPoints.Count)
        {
            int j = 0;
            while (j < patchOfSoil.PlantPoints[i].PlantEatingPoints.Count)
            {
                if (!patchOfSoil.PlantPoints[i].PlantEatingPoints[j].HasRat && patchOfSoil.PlantPoints[i].HasCrop)
                    return true;

                j++;
            }
            i++;
        }

        return false;
    }

    // Checks if the any of the plantpoints in the soil has crops
    private bool soilHasCrops()
    {
        int i = 0;
        while (i < patchOfSoil.PlantPoints.Count)
        {
            if (patchOfSoil.PlantPoints[i].HasCrop)
                return true;
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
            ratScript = rat.GetComponent<Rat_Movement_Logic>();
            setRatDestinations();
            ratScript.chooseNewTarget();
            rats.Add(rat);
        }
    }

    // Sets the destinations for each rat created
    private void setRatDestinations()
    {
        for (int i = 0; i < patchOfSoil.PlantPoints.Count; i++)
        {
            if(patchOfSoil.PlantPoints[i].HasCrop)
                ratScript.setDestinations(patchOfSoil.PlantPoints[i].PlantEatingPoints);
        }
    }
}
