using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    NavMeshSurface thisNavMeshSurface;
    // Start is called before the first frame update
    void Start()
    {
        thisNavMeshSurface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        thisNavMeshSurface.BuildNavMesh();
    }
}
