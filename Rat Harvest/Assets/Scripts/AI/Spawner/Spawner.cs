using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private ObjectPooler objectPooler;

    [SerializeField]
    private List<Transform> spawningPositions_Transform_List = new List<Transform>();

    private Transform spawningPosition_Transform;

    [SerializeField] private Soil patchOfSoil;
    public Soil PatchOfSoil { get { return this.patchOfSoil; } }

    [SerializeField] private GameObject objectToSpawn;
   
    // List to store the rats that spawn 
    private List<GameObject> rats = new List<GameObject>();

    //GameObject to store the specific rat that has been spawned
    private GameObject rat;

    // Script of the rat chosen
    private Rat_Movement_Logic ratScript;

    float time = 0;

    [SerializeField]
    private float minimumSpawningTime = 3;

    [SerializeField]
    private float maximumSpawningTime = 5;

    private float spawningTime;

    [SerializeField]
    private bool startRound;
    public bool StartRound { set { this.startRound = value; } } 

    [SerializeField]
    private GameObject despawnPoint;

    private void Awake()
    {
        objectPooler = ObjectPooler.instance;
    }

    private void Start()
    {
        SetNewSpawningTime();
    }

    private void Update()
    {
        if (startRound)
        {
            time += Time.deltaTime;
            if (time >= spawningTime)
            {
                SpawnRat();
                SetNewSpawningTime();
                time = 0;
            }
        }
    }

    // Spawns a rat
    private void SpawnRat()
    {
       

        if (ArePlantEatingPointsAvailables())
        {
            SelectSpawningPosition();
            objectPooler.spawnFromPool(objectToSpawn.name, spawningPosition_Transform.position, spawningPosition_Transform.rotation, out rat);
            setRatValues(rat);
            rats.Add(rat);
        }
    }

    // Checks if there's any plant eating point available
    private bool ArePlantEatingPointsAvailables()
    {
        int i = 0;
        while (i < patchOfSoil.PlantPoints.Count)
        {
            if (patchOfSoil.PlantPoints[i].HasCrop)
            {
                int j = 0;
                while (j < patchOfSoil.PlantPoints[i].PlantEatingPoints.Count)
                {
                    if (!patchOfSoil.PlantPoints[i].PlantEatingPoints[j].HasRat)
                        return true;

                    j++;
                }
            }
            i++;
        }

        return false;
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


    private void setRatValues(GameObject rat)
    {
        ratScript = rat.GetComponent<Rat_Movement_Logic>();
        setRatDestinations();
        ratScript.chooseNewTarget();
        ratScript.DespawnPoint = despawnPoint;
    }

    private void SetNewSpawningTime()
    {
        spawningTime = Random.Range(minimumSpawningTime,maximumSpawningTime);
    }

    private void SelectSpawningPosition() {

        int i = Random.Range(0, spawningPositions_Transform_List.Count);
        spawningPosition_Transform = spawningPositions_Transform_List[i];
    }
}
