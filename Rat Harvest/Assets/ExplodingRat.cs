using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingRat : MonoBehaviour
{
    [SerializeField] private float destroyDelay;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float radius;

    private void Start()
    {
        Explode();
    }

    private void Explode()
    {
        foreach (Transform t in transform)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Hi");
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }

        }
    }
}
