using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Behaviour : MonoBehaviour, IPooledObject
{
    // Variable to set the time that must pass for the plant to grow
    [SerializeField] private float growingTime = 5f;

    // A variable to set the number of states the plant has
    [SerializeField] private int numOfStates = 3;

    // A variable to set the plant's health
    [SerializeField] private int Health;
    private int plantHealth;

    // A variable to know which is the critical health of the plant
    [Tooltip("The health conisdered critical for the plant")]
    [SerializeField] private int plantCriticalHealth;

    // A variable to know the current state of the plant
    private int currentState = 0;

    // Public list to set the different models of the plant
    [SerializeField] private List<GameObject> plantModels;
    public List<GameObject> PlantModels { get { return plantModels; } }

    // Current plant
    private GameObject currentPlant;
    public GameObject CurrentPlant { set { this.currentPlant = value; } }

    // A list to keep th epoints per state the plant gives
    [SerializeField] private List<int> pointsPerState;

    // Points of the current state
    private int currentPoints = 0;
    public int CurrentPoints { get { return this.currentPoints; } }

    // Variable to have the count of the time passed
    //private float time;

    // A variable to store the pool
    ObjectPooler objectPooler = ObjectPooler.instance;

    //A variable to know the specific Plantpoint where this plant is planted
    private GameObject plantPoint;
    public GameObject PlantPoint { get { return this.plantPoint; } set { this.plantPoint = value; } }

    [SerializeField]
    private bool startRound;
    public bool StartRound { set { this.startRound = value; } }

    private Plant_Health_Logic plantHealthScript;


    // Method inherited form the IpooledObject interface
    public void OnObjectSpawn()
    {
        initializePlant();
        StartCoroutine(CountTime());
        StartCoroutine(PlantCriticalState());
    }

    private IEnumerator CountTime()
    {
        yield return new WaitUntil(() => startRound);
   
        yield return new WaitForSeconds(growingTime);
        Grow();

        if (currentState < numOfStates - 1)
            StartCoroutine(CountTime());
    }

    // Method with the growth logic of the plant
    private void Grow()
    {  
        updateModel();
        updatePoints();
    }

    // Method to change the model of the plant
    private void updateModel()
    {
        currentPlant.transform.parent = null;

        objectPooler.killGameObject(currentPlant);
        currentState++;
        objectPooler.spawnFromPool(plantModels[currentState].name, transform.position, transform.rotation, out currentPlant);

        plantHealthScript = currentPlant.GetComponent<Plant_Health_Logic>();
        plantHealthScript.updateHealth(plantHealth);
        currentPlant.transform.parent = this.gameObject.transform;
    }

    // Method to update the points of the current model
    private void updatePoints()
    {
        currentPoints = pointsPerState[currentState];
    }

    // Method to initialize the plant behaviour
    private void initializePlant()
    {
        objectPooler.spawnFromPool(plantModels[currentState].name, transform.position, transform.rotation, out currentPlant);
        plantHealthScript = currentPlant.GetComponent<Plant_Health_Logic>();
        currentPlant.transform.parent = this.gameObject.transform;
        currentPoints = pointsPerState[currentState];
        plantHealth = Health;
    }

    // Method to reset all the variables when the player harvests
    public void resetPlant()
    {
        currentState = 0;
        currentPoints = 0;

        plantPoint.GetComponent<PlantPoint>().HasCrop = false;
        plantPoint.GetComponent<PlantPoint>().DisablePlantEatingPoints();

        StopCoroutine(CountTime());
        StopCoroutine(PlantCriticalState());

        currentPlant.GetComponent<PlantModel>().DisableOutline();
        currentPlant.transform.parent = null;

        objectPooler.killGameObject(currentPlant);
    }

    // The plant starts to shine
    private IEnumerator PlantCriticalState()
    {
        yield return new WaitUntil(() => plantHealth < plantCriticalHealth);
        currentPlant.GetComponent<PlantModel>().EnableOutline();
        yield return null;
    }

    // Substracts health from the plant
    public void SubPlantHealth(int damage)
    {
        plantHealth -= damage;
        //Debug.Log("Plant Health = " + plantHealth);
        plantHealthScript.updateHealth(plantHealth);

        if (plantHealth <= 0)
        {
            resetPlant();    
            objectPooler.killGameObject(this.gameObject);
        }          
    }

    // Checks if the plant can be harvested
    public bool CanBeHarvested()
    {
        bool b = false;

        if (currentState > 0)
            b = true;
        else
            b = false;

        return b;
    }
}