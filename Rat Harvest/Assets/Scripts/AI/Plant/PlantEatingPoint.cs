﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEatingPoint : MonoBehaviour
{
    private float time = 0;

    private PlantPoint plantPoint;

    private Rat_Movement_Logic ratScript;
    public Rat_Movement_Logic RatScript { get { return this.ratScript; }  set { this.ratScript = value; } }

    private bool hasRat = false;
    public bool HasRat { get { return this.hasRat; } set { this.hasRat = value; } }

    private bool plantEatingPointReached = false;
    public bool PlantEatingPointReached { set { this.plantEatingPointReached = value; } }

    private bool plantGrowing = false;
    public bool PlantGrowing { get { return this.plantGrowing; } set { this.plantGrowing = value; } }

    [SerializeField] private float timePerAttack = 3;

    // Start is called before the first frame update
    void Start()
    {
        plantPoint = gameObject.transform.parent.gameObject.GetComponent<PlantPoint>();
        StartCoroutine(EatPlant());
    }

    // Update is called once per frame
    private void Update()
    {
        
            
    }

    private void AttackPlant()
    {
        plantPoint.Plant.GetComponent<Plant_Behaviour>().SubPlantHealth(ratScript.Damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rat_Movement_Logic>() != null)
            plantEatingPointReached = true;
    }

    private IEnumerator EatPlant()
    {
        yield return new WaitUntil(() => plantEatingPointReached);

        AttackPlant();

        yield return new WaitForSeconds(timePerAttack);
        StartCoroutine(EatPlant());
    }

    //private void EatPlant()
    //{
    //    time += Time.deltaTime;
    //    if (time >= timePerAttack)
    //    {
    //        AttackPlant();
    //        time = 0;
    //    }
    //}

}
