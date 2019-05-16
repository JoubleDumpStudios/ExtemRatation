using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsLogic : MonoBehaviour
{
    [SerializeField]
    private float smoothTime;


    private Vector3 refVel;
 
    private Transform initialPositionTransform;

    [SerializeField]
    private Transform endPositionTransform;
    // Start is called before the first frame update
    void Start()
    {
        initialPositionTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, endPositionTransform.position, ref refVel, smoothTime);
    }
}
