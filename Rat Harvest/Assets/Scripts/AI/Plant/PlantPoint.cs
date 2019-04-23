using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPoint : MonoBehaviour
{
    // A variable to know if the soil has a crop or not
    private bool hasCrop = false;
    public bool HasCrop { get { return this.hasCrop; } set { this.hasCrop = value; } }

    // A list to store the eating plant stand points around a plant
    [SerializeField] private List<GameObject> eatingPlantPoints;
    public List<GameObject> EatingPlantPoints { get { return this.eatingPlantPoints; } }
}
