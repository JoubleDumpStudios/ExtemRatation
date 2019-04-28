using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    [SerializeField] private Soil patchOfSoil;
    public Soil PatchOfSoil { get { return this.patchOfSoil; } }

    private Spawner spawner;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
    }
}
