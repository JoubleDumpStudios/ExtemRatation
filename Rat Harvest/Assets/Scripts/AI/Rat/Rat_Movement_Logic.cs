using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat_Movement_Logic : MonoBehaviour, IPooledObject
{

    // The damage caused by the rats
    [SerializeField] private int damage;
    public int Damage { get { return this.damage; } }

    [SerializeField] private float secondsToAttack;

    private List<GameObject> destinations = new List<GameObject>();// must contain all the possible destinations we want for the movable objects

    NavMeshAgent _navMeshAgent;

    bool lookForNewTarget = false;

    int targetIndex = 0;

    bool noDestinationsAvailables = false;

    [SerializeField]
    private PlantEatingPoint ratTarget;
    public PlantEatingPoint RatTarget { get { return this.ratTarget; } }

    private GameObject despawnPoint;
    public GameObject DespawnPoint { set { this.despawnPoint = value; } }

    private bool returningHome;

    public Animator ratAnimator;
    public Animator RatAnimator { get { return this.ratAnimator; } }

    public void OnObjectSpawn()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();//allos the object to use the navmesh component and options
        _navMeshAgent.speed = 3.5f;
        ratAnimator = GetComponentInChildren<Animator>();
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

    public void chooseNewTarget() //calls the navMeshMethod with the nearest and available position, sets the value of the destination to unavailable
    {
        if (areDestinationsAvailables())
        {
            GameObject selectedDestination = destinations[calculateNearestObjectIndex()];
            ratTarget = selectedDestination.GetComponent<PlantEatingPoint>();
            ratTarget.HasRat = true;
            ratTarget.RatScript = this.gameObject.GetComponent<Rat_Movement_Logic>();
            ratTarget.TimePerAttack = secondsToAttack;
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


    public void ratBackHome()
    {
        returningHome = true;

        ResetRatDestinations();

        navMeshSetDestination(despawnPoint);
    }

    public void killRat()
    {
        if (!returningHome)
        {
            ratTarget.HasRat = false;
            ResetRatDestinations();
        }

        DespawnRat();
    }

    public void DespawnRat()
    {
        //if (returningHome) returningHome = false;
        //ObjectPooler.instance.killGameObject(this.gameObject);
        StartCoroutine(KilledRatAnim());
    }

    public void ResetRatDestinations()
    {
        //ratTarget.HasRat = false;
        destinations.Clear();
        ratTarget.PlantEatingPointReached = false;

        if(ratAnimator != null)
            ratAnimator.SetBool("Attacking", false);
    }

    private IEnumerator KilledRatAnim()
    {
        if (returningHome) returningHome = false;

        if (ratAnimator != null)
        {
            _navMeshAgent.speed = 0;
            ratAnimator.SetBool("Killed", true);
        }
            

        yield return new WaitForSeconds(0.917f);

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
