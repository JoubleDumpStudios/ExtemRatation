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

    private Animator ratAnimator;
    public Animator RatAnimator { get { return this.ratAnimator; } }

    [SerializeField] private GameObject normalRat;

    [SerializeField] private float explodeDistance;
    public float ExplodeDistance { get { return this.explodeDistance; } }

    [SerializeField] private float explosionDuration;

    public GameObject ExplodingRat;

    private float navSpeed;

    public void OnObjectSpawn()
    {
        normalRat.SetActive(true);
        _navMeshAgent = this.GetComponent<NavMeshAgent>();//allos the object to use the navmesh component and options
        ratAnimator = GetComponentInChildren<Animator>();
        navSpeed = _navMeshAgent.speed;
        _navMeshAgent.speed = navSpeed;
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

    public void killRat(float distance)
    {
        if (!returningHome)
        {
            ratTarget.HasRat = false;
            ResetRatDestinations();
        }

        DespawnRat(distance);
    }

    public void DespawnRat(float distance)
    {
        //if (returningHome) returningHome = false;
        //ObjectPooler.instance.killGameObject(this.gameObject);
        StartCoroutine(KilledRatAnim(distance));
    }

    public void ResetRatDestinations()
    {
        destinations.Clear();
        ratTarget.PlantEatingPointReached = false;
        //Debug.Log(ratTarget.plantEatingPointReached + "is false");

        if(ratAnimator != null)
            ratAnimator.SetBool("Attacking", false);
    }

    private IEnumerator KilledRatAnim(float distance)
    {
        //if (returningHome) returningHome = false;
        _navMeshAgent.speed = 0;

        if (!returningHome)
        {
            if (distance <= explodeDistance)
            {
                //_navMeshAgent.speed = 0;
                normalRat.SetActive(false);
                var ExplodeClone = Instantiate(ExplodingRat, new Vector3(gameObject.transform.position.x, 0.8f, gameObject.transform.position.z), gameObject.transform.rotation);
                yield return new WaitForSeconds(explosionDuration);
                Destroy(ExplodeClone);
            }
            else
            {
                if (ratAnimator != null)
                {
                    //_navMeshAgent.speed = 0;
                    ratAnimator.SetBool("Killed", true);
                }
                //Debug.Log("Distance = " + distance);
                yield return new WaitForSeconds(0.917f);
            }
        }
        else
        {
            if (distance <= explodeDistance)
            {
                //_navMeshAgent.speed = 0;
                normalRat.SetActive(false);
                var ExplodeClone = Instantiate(ExplodingRat, new Vector3(gameObject.transform.position.x, 0.8f, gameObject.transform.position.z), gameObject.transform.rotation);
                yield return new WaitForSeconds(explosionDuration);
                Destroy(ExplodeClone);
            }

            returningHome = false;
        }
            

        ratTarget.PlantEatingPointReached = false;
        _navMeshAgent.speed = navSpeed;
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
