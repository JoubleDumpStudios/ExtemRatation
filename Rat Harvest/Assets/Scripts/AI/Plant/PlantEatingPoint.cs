﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEatingPoint : MonoBehaviour
{
    //float time = 0;

    //[SerializeField]
    //private float eatingTime = 3;

    private PlantPoint plantPoint;

    private Rat_Movement_Logic ratScript;
    public Rat_Movement_Logic RatScript { set { this.ratScript = value; } }

    private bool hasRat = false;
    public bool HasRat { get { return this.hasRat; } set { this.hasRat = value; } }

    private bool plantGrowing = false;
    public bool PlantGrowing { get { return this.plantGrowing; } set { this.plantGrowing = value; } }

    // Start is called before the first frame update
    void Start()
    {
        plantPoint = gameObject.transform.parent.gameObject.GetComponent<PlantPoint>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (ratEatingMe )
        //{
        //    time += Time.deltaTime;
        //    if (time >= eatingTime)
        //    {
        //        ratEatingMe = false;
        //        rat.eatPlant(this.gameObject);
        //        rat.chooseNewTarget();
        //        time = 0;
        //    }

        //}
        if (hasRat)
            AttackPlant();
    }

    private void AttackPlant()
    {
        plantPoint.Plant.GetComponent<Plant_Behaviour>().SubPlantHealth(ratScript.Damage);
    }

}
