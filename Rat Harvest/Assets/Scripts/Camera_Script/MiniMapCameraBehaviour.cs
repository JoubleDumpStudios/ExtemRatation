using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform playerTransform;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
            this.transform.position = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
        else
            Debug.Log("CAMERA IS NOT FOLLOWING THE PLAYER, ASIGN THE PLAYER TO THE CAMERA; THE REFERENCE IS MISSING");
    }
}
