using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_Movement_Logic : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> destinations;// must contain all the possible destinations we want for the movable objects

    NavMeshAgent _navMeshAgent;

    bool lookForNewTarget = false;

    int targetIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();//allos the object to use the navmesh component and options
        setDestination(destinations[calculateNearestObjectIndex()]);//calls the method that allows to find the first destination to go
    }

    private void setDestination(GameObject obstacle)
    {
        _navMeshAgent.SetDestination(obstacle.transform.position);//use the navMesh options to set a destination for the Nav Mesh Agent
    }

    // Update is called once per frame
    void Update()
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
        return targetIndex;
    }


    public void chooseNewTarget()//allow us to call the methods to choose a new target, it is going to be called from the plat that it is been eating once is eated
    {
        if (destinations.Count > 0)
            setDestination(destinations[calculateNearestObjectIndex()]);
    }

    public void eatPlant(GameObject plant)//eats the plant asociated with this destination
    {
        destinations.Remove(plant);
    }
}
