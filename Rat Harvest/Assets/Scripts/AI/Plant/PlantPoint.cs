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

    // A variable to access to the material
    private cakeslice.Outline outline;


    private void Start()
    {
        outline = GetComponent<cakeslice.Outline>();
        DisableOutline();
    }

    public void EnablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)       
            plantEatingPoints[i].PlantGrowing = true;
    }

    public void DisablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            plantEatingPoints[i].PlantGrowing = false;
            plantEatingPoints[i].PlantEatingPointReached = false;

            if (plantEatingPoints[i].HasRat)
            {
                plantEatingPoints[i].RatScript.ratBackHome();
                plantEatingPoints[i].HasRat = false;        
            }
        }
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

}
