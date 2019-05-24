using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    // A list to store the eating plant stand points around a plant
    [SerializeField] private List<PlantEatingPoint> plantEatingPoints;
    public List<PlantEatingPoint> PlantEatingPoints { get { return this.plantEatingPoints; } }

    // A variable to store the specific plant in the plantPoint
    private GameObject plant;
    public GameObject Plant { get { return this.plant; } set { this.plant = value; } }

    // A variable to know if the soil has a crop or not
    private bool hasCrop = false;
    public bool HasCrop { get { return this.hasCrop; } set { this.hasCrop = value; } }

    private bool roundStarted = false;
    public bool RoundStarted { get { return this.roundStarted; } set { this.roundStarted = value; } }


    private Icon_Plant_Behaviour iconPlantBehaviouScript;

    private void Start()
    {
        iconPlantBehaviouScript = GetComponentInChildren<Icon_Plant_Behaviour>();
        iconPlantBehaviouScript.gameObject.SetActive(false);
    }
    //public void EnablePlantEatingPoints()
    //{
    //    for (int i = 0; i < plantEatingPoints.Count; i++)       
    //        plantEatingPoints[i].PlantGrowing = true;
    //}

    public void DisablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            plantEatingPoints[i].DisablePantEatingPoint();
           
            //plantEatingPoints[i].PlantGrowing = false;
            //plantEatingPoints[i].PlantEatingPointReached = false;

            //if (plantEatingPoints[i].HasRat)
            //{
            //    plantEatingPoints[i].RatScript.ratBackHome();
            //    plantEatingPoints[i].HasRat = false;        
            //}
        }
        disabePlantIcon();
    }

    public void enablePlantIcon()
    {
        iconPlantBehaviouScript.gameObject.SetActive(true);
    }

    private void disabePlantIcon()
    {
        iconPlantBehaviouScript.gameObject.SetActive(false);
    }

}
