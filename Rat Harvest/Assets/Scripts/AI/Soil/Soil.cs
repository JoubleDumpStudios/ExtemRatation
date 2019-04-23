using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    [SerializeField] private List<PlantPoint> plantStandPoints;
    public List<PlantPoint> PlantPoints { get { return this.plantStandPoints; } }

}
