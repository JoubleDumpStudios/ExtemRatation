using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{

    Transform startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform;
    }

    // Update is called once per frame
    float timeCounter = 0;

    [SerializeField]
    private float vel = 0;

    [SerializeField]
    private float radius = 0;

    void Update()
    {
        timeCounter += vel * Time.deltaTime; // multiply all this with some speed variable (* speed);
        float x = radius * Mathf.Cos(timeCounter);
        float y = 0;
        float z = radius * Mathf.Sin(timeCounter); ;
        transform.position = startingPosition.position + new Vector3(x, y, z);
    }
}
