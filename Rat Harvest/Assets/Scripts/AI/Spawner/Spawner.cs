using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    [SerializeField] private Soil patchOfSoil;
    [SerializeField] private GameObject objectToSpawn;
   

    // List to store the rats that spawn 
    private List<GameObject> rats = new List<GameObject>();

    //GameObject to store the specific rat that has been spawned
    private GameObject rat;
    
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.instance;
        spawnRat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnRat()
    {
        objectPooler.spawnFromPool(objectToSpawn.name, transform.position, transform.rotation, out rat);

        for (int i = 0; i < patchOfSoil.PlantPoints.Count; i++)
            rat.GetComponent<Rat_Movement_Logic>().setDestinations(patchOfSoil.PlantPoints[i].EatingPlantPoints);
 
        rats.Add(rat);

        Debug.Log("I'm here");
    }

    private void declareRat()
    {

    }

    private void assignListToTheRat()
    {

    }
}
