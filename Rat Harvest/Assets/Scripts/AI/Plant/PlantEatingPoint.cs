using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEatingPoint : MonoBehaviour
{
    private PlantPoint plantPoint;

    private Rat_Movement_Logic ratScript;
    public Rat_Movement_Logic RatScript { get { return this.ratScript; }  set { this.ratScript = value; } }

    private bool hasRat = false;
    public bool HasRat { get { return this.hasRat; } set { this.hasRat = value; } }

    private bool plantEatingPointReached = false;
    public bool PlantEatingPointReached { set { this.plantEatingPointReached = value; } }

    private bool plantGrowing = false;
    public bool PlantGrowing { get { return this.plantGrowing; } set { this.plantGrowing = value; } }

    private float timePerAttack;
    public float TimePerAttack { set { this.timePerAttack = value; } }

    private Icon_Plant_Behaviour icon_Plant_Behaviour;



    // Start is called before the first frame update
    void Start()
    {
        icon_Plant_Behaviour = gameObject.transform.parent.gameObject.GetComponentInChildren<Icon_Plant_Behaviour>();
        plantPoint = gameObject.transform.parent.gameObject.GetComponent<PlantPoint>();
        StartCoroutine(EatPlant());
    }

    // Method that substracts life from the plant
    private void AttackPlant()
    {
        plantPoint.Plant.GetComponent<Plant_Behaviour>().SubPlantHealth(ratScript.Damage);

        icon_Plant_Behaviour.PlantAttacked();
    }

    // Coroutine that executes the plant life substraction every 'x' seconds
    private IEnumerator EatPlant()
    {
        yield return new WaitUntil(() => plantEatingPointReached);

        AttackPlant();

        yield return new WaitForSeconds(timePerAttack);
        StartCoroutine(EatPlant());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rat_Movement_Logic>() != null)
            plantEatingPointReached = true;
    }

}
