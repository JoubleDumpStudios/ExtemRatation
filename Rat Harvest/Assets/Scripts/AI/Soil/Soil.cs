using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    [SerializeField] private List<PlantPoint> plantPoints;
    public List<PlantPoint> PlantPoints { get { return this.plantPoints; } }

    private Spawner spawner;
    public Spawner Spawner { get { return this.spawner; } set { this.spawner = value; } }
}
