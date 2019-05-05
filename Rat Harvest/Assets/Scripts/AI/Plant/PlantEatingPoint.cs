using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEatingPoint : MonoBehaviour
{
    private float time = 0;

    private PlantPoint plantPoint;

    private Rat_Movement_Logic ratScript;
    public Rat_Movement_Logic RatScript { set { this.ratScript = value; } }

    private bool hasRat = false;
    public bool HasRat { get { return this.hasRat; } set { this.hasRat = value; } }

    private bool plantEatingPointReached = false;

    private bool plantGrowing = false;
    public bool PlantGrowing { get { return this.plantGrowing; } set { this.plantGrowing = value; } }

    [SerializeField] private float timePerAttack = 3;

    // Start is called before the first frame update
    void Start()
    {
        plantPoint = gameObject.transform.parent.gameObject.GetComponent<PlantPoint>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (plantEatingPointReached)
            EatPlant();
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

    //private IEnumerator EatPlant()
    //{
    //    float time = 0f;

    //    AttackPlant();

    //    while (time < timePerAttack)
    //    {
    //        time += Time.deltaTime / timePerAttack;
    //        yield return null;
    //    }
    //}

    private void EatPlant()
    {
        time += Time.deltaTime;
        if (time >= timePerAttack)
        {
            AttackPlant();
            time = 0;
        }
    }

}
