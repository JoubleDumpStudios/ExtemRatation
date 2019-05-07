using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    // A variable to store the specific plant in the plantPoint
    private GameObject plant;
    public GameObject Plant { get { return this.plant; } set { this.plant = value; } }

    // A variable to know if the soil has a crop or not
    private bool hasCrop = false;
    public bool HasCrop { get { return this.hasCrop; } set { this.hasCrop = value; } }

    // A list to store the eating plant stand points around a plant
    [SerializeField] private List<PlantEatingPoint> plantEatingPoints;
    public List<PlantEatingPoint> PlantEatingPoints { get { return this.plantEatingPoints; } }

    private Spawner Soilspawner;

    private void Start()
    {
        Soilspawner = this.gameObject.transform.parent.gameObject.GetComponent<Soil>().Spawner;
    }

    public void EnablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)       
            plantEatingPoints[i].PlantGrowing = true;


        Soilspawner.ActivateSpawner = true;
    }


    public void DisablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            plantEatingPoints[i].PlantGrowing = false;
            plantEatingPoints[i].PlantEatingPointReached = false;

        }

        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            if (plantEatingPoints[i].HasRat)
            {
                plantEatingPoints[i].RatScript.ratDied();
                ObjectPooler.instance.killGameObject(plantEatingPoints[i].RatScript.gameObject);
                plantEatingPoints[i].HasRat = false;
            }
                        
        }

        Soilspawner.ActivateSpawner = false;
    }

}
