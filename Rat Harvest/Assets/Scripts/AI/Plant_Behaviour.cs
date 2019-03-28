using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Behaviour : MonoBehaviour
{
    // Variable to set the time that must pass for the plant to grow
    [SerializeField]
    private float growingTime = 5f;

    // A variable to set the number of states the plant has
    [SerializeField]
    private int numOfStates = 3;

    // A variable to know the current state of the plant
    private int currentState = 0;

    // Public list to set the different models of the plant
    [SerializeField]
    private List<GameObject> plantModels;

    // A list to keep the model active at a specific time
    private List<GameObject> currentModels = new List<GameObject>();

    // Variable to have the count of the time passed
    private float time;

    private void Start()
    {      
        InitializeModels();

        currentModels[currentState].SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        CountTime();
    }

    // Method with counts the time for the plant to grow
    private void CountTime()
    {
        time += Time.deltaTime;

        if (time >= growingTime && currentState < numOfStates - 1)
            Grow();
    }

    // Method with the growth logic of the plant
    private void Grow()
    {
        time = 0f;

        currentModels[currentState].SetActive(false);

        currentState++;

        ChangeModel();
        
    }

    // Method to change the model of the plant
    private void ChangeModel()
    {
        currentModels[currentState].SetActive(true);
    }

    // A method to instantiate the models for the plant
    private void InitializeModels()
    {
        for (int i = 0; i < plantModels.Count; i++)
        {
            currentModels.Add(Instantiate(plantModels[i], transform.position, transform.rotation));
            currentModels[i].SetActive(false);
        }
    }

}
    

