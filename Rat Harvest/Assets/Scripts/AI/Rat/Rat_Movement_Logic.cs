using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_Movement_Logic : MonoBehaviour
{
    [SerializeField]
    List<GameObject> destinations = new List<GameObject>();// must contain all the possible destinations we want for the movable objects

    NavMeshAgent _navMeshAgent;

    bool lookForNewTarget = false;

    int targetIndex = 0;

    bool noDestinationsAvailables = false;

    [SerializeField]
    private PlantEatingPoint ratTarget;

    private void Awake()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();//allos the object to use the navmesh component and options
    }

    void Start()
    {
    }


    int calculateNearestObjectIndex()//allow us to select the nearest destination from the list of destinations
    {
        float distance = 0;
        float nearestDistance = 200000;

        for (int i = 0; i<destinations.Count; i++)
        {
                distance = Vector3.Distance(this.transform.position, destinations[i].transform.position);

                if (nearestDistance > distance)
                {
                    nearestDistance = distance;
                    targetIndex = i;
                }
        }

        return targetIndex;//returns the index on the list of the destination that is choosed by the rat spawned
    }

    public void chooseNewTarget()//calls the navMeshMethod with the nearest and available position, sets the value of the destination to unavailable
    {
        if (areDestinationsAvailables())
        {
            GameObject selectedDestination = destinations[calculateNearestObjectIndex()];
            ratTarget = selectedDestination.GetComponent<PlantEatingPoint>();
            //selectedDestination.GetComponent<PlantEatingPoint>().HasRat = true;
            ratTarget.HasRat = true;
            navMeshSetDestination(selectedDestination);
        }
    }

    public bool areDestinationsAvailables()
    {
        if (destinations.Count > 0)
            return true;
        return false;
    }

    private void navMeshSetDestination(GameObject obstacle)//default nav mesh method
    {
        _navMeshAgent.SetDestination(obstacle.transform.position);//use the navMesh options to set a destination for the Nav Mesh Agent
    }


    public void setDestinations(List<PlantEatingPoint> plantEatingPoints)
    {

        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            if (!plantEatingPoints[i].HasRat)
                destinations.Add(plantEatingPoints[i].gameObject);
        }    
    }

    public void eatPlant(GameObject plant)//eats the plant asociated with this destination
    {
        destinations.Remove(plant);
    }

    public void ratDied()
    {
        ratTarget.HasRat = false;
        destinations.Clear();
        ObjectPooler.instance.killGameObject(this.gameObject);

    }

    //private bool hasDestinations(List<PlantEatingPoint> plantEatingPoints)
    //{
    //    int i = 0;
    //    while (i < plantEatingPoints.Count)
    //    {
    //        if (!plantEatingPoints[i].HasRat)
    //            return true;
    //        i++;
    //    }

    //    return false;
    //}


    //public void checkAndSetRatDestinations(List<PlantEatingPoint> plantEatingPoints)
    //{
    //    if (hasDestinations(plantEatingPoints))
    //        setDestinations(plantEatingPoints);
    //}

}
