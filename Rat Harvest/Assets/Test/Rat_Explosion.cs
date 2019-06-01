using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Explosion : MonoBehaviour
{

    public float radius = 5.0F;
    public float power = 200.0F;
    public float upwardsModifier = 0.2f;
    public Transform explosionPoint;

    private void Start()
    {
        Explode();
        AkSoundEngine.PostEvent("Rat_Explode", gameObject);
    }
    
    private void Explode()
    {
        Vector3 explosionPos = explosionPoint.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, upwardsModifier);
        }
    }
}
