using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_Movement_Logic : MonoBehaviour
{

    [SerializeField]
    Transform _destination;

    

    public List<GameObject> destinations;

    NavMeshAgent _navMeshAgent;

    public bool lookForNewTarget = false;

    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        
        setDestination(destinations[calculateNearestObjectIndex()]);
    }

    private void setDestination(GameObject obstacle)
    {
        //Vector3 targetVector = _destination.transform.position;
        _navMeshAgent.SetDestination(obstacle.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (lookForNewTarget)
        {
            setDestination(destinations[calculateNearestObjectIndex()]);
            lookForNewTarget = false;
        }
    }

    int calculateNearestObjectIndex()
    {
        int indicator = 0;
        float distance = 0;
        float nearestDistance = 2000;

        for (int i = 0; i<destinations.Count; i++)
        {
            if (destinations[i] != null) {
                distance = Vector3.Distance(this.transform.position, destinations[i].transform.position);

                if (nearestDistance > distance)
                {
                    nearestDistance = distance;
                    indicator = i;
                }
            }
        }
        return indicator;
    }
}
