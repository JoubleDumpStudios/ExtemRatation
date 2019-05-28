using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Rat_Movement_Logic ratMovementScript = other.gameObject.GetComponent<Rat_Movement_Logic>();
        if(ratMovementScript != null)
        {
            ratMovementScript.DespawnRat(1000);
        }
    }
}
