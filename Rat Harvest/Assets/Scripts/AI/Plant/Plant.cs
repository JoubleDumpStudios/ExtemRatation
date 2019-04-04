using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private int points;
    public int Points { get { return this.points; } set { this.points = value; } }

    private GameObject plant;
    public GameObject Root_Plant { get { return this.plant; } set { this.plant = value; } }
}
