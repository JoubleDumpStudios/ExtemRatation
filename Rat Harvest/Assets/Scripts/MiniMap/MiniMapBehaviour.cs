using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapBehaviour : MonoBehaviour
{

    RectTransform originalTransform;


    [SerializeField]
    private Transform zoomedMinimapTransform;

    [SerializeField]
    private float zoomTime;
    [SerializeField]
    private float zoomSpeed;
    private Vector3 zoomSpeed_;




    private float originalPositionX;
    private float originalPositionY;
    private float originalScaleX;

    public bool increasing_b = false;


    public float zoomScaleX;




    // Start is called before the first frame update
    void Start()
    {
        originalTransform = GetComponent<RectTransform>();
        originalPositionX = originalTransform.position.x;
        originalPositionY = originalTransform.position.y;

        originalScaleX = originalTransform.localScale.x;

        zoomScaleX = zoomedMinimapTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    this.transform.position = new Vector3(zoomedMinimapTransform.position.x, zoomedMinimapTransform.position.y, transform.position.z);
        //    this.transform.localScale = new Vector3(zoomedMinimapTransform.transform.localScale.x, zoomedMinimapTransform.transform.localScale.y, transform.localScale.z);
        //}

        if (Input.GetKey(KeyCode.M))
        {
            increasing_b = true;
        }

        if(Input.GetKeyUp(KeyCode.M))
            increasing_b = false;

        if (increasing_b)
            increasing();
        else decreasing();



    }

    private void increasing()
    {
        transform.position = Vector3.SmoothDamp(transform.position, zoomedMinimapTransform.position, ref zoomSpeed_, zoomTime, zoomSpeed);
 
        //transform.localScale = Vector3.SmoothDamp(transform.lossyScale, 
        //    new Vector3(zoomScaleX, zoomScaleX, 1),
        //    ref zoomSpeed_, zoomTime, zoomSpeed);
    }

    private void decreasing()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(originalPositionX, originalPositionY, transform.position.z), ref zoomSpeed_, zoomTime, zoomSpeed);

        //transform.localScale = Vector3.SmoothDamp(transform.localScale, 
        //    new Vector3(originalScaleX, originalScaleX, transform.localScale.z),
        //    ref zoomSpeed_, zoomTime, zoomSpeed);
    }
}
