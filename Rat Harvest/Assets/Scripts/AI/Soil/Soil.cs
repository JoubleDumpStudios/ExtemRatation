using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    [SerializeField] private List<PlantPoint> plantStandPoints;
    [SerializeField] private Spawner spawner;
    public Spawner Spawner { get { return this.spawner; } }
    public List<PlantPoint> PlantStandPoints { get { return this.plantStandPoints; } }
}
