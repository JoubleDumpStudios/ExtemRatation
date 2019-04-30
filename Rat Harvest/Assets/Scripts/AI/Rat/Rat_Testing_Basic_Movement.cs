using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_Testing_Basic_Movement : MonoBehaviour
{

    NavMeshAgent _navMeshAgent;
    public GameObject destination;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.SetDestination(destination.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
