using System.Collections;
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

    // spawns a rat
    private void spawnRat()
    {
        objectPooler.spawnFromPool(objectToSpawn.name, transform.position, transform.rotation, out rat);
        setRatDestinations();

        rat.GetComponent<Rat_Movement_Logic>().chooseNewTarget();

        rats.Add(rat);
    }

    // Sets the destinations for each rat created
    private void setRatDestinations()
    {
        for (int i = 0; i < patchOfSoil.PlantPoints.Count; i++)
            rat.GetComponent<Rat_Movement_Logic>().setDestinations(patchOfSoil.PlantPoints[i].PlantEatingPoints);
    }
}
