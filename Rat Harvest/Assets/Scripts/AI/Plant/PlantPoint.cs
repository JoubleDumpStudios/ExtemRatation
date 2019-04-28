using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    // A variable to know if the soil has a crop or not
    private bool hasCrop = false;
    public bool HasCrop { get { return this.hasCrop; } set { this.hasCrop = value; } }

    // A list to store the eating plant stand points around a plant
    [SerializeField] private List<PlantEatingPoint> plantEatingPoints;
    public List<PlantEatingPoint> PlantEatingPoints { get { return this.plantEatingPoints; } }

    private Soil soil;
    private Spawner spawner;

    private void Start()
    {
        soil = GetComponentInParent<Soil>();
        spawner = soil.Spawner;
    }

    public void EnablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            plantEatingPoints[i].PlantGrowing = true;
        }
        spawner.ActivateSpawner = true;
    }


    public void DisablePlantEatingPoints()
    {
        for (int i = 0; i < plantEatingPoints.Count; i++)
        {
            plantEatingPoints[i].PlantGrowing = false;
        }

        spawner.ActivateSpawner = false;
    }

}
