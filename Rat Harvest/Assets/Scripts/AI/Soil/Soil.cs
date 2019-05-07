using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    [SerializeField] private List<PlantPoint> plantStandPoints;
    public List<PlantPoint> PlantStandPoints { get { return this.plantStandPoints; } }

    private Spawner spawner;
    public Spawner Spawner { get { return this.spawner; } set { this.spawner = value; } }
}
