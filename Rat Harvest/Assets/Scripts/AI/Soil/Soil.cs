using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    // List with the plantPoints inside the soil
    [SerializeField] private List<PlantPoint> plantPoints;
    public List<PlantPoint> PlantPoints { get { return this.plantPoints; } }
}
