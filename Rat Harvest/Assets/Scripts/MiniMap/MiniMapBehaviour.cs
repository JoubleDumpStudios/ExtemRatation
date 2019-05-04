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

    [SerializeField]
    private float increasingScaleZoomSpeed;

    private Vector3 zoomSpeed_vector;
    private float zoomSpeed_;

    float scale;



    private float originalPositionX;
    private float originalPositionY;
    private float originalScaleX;

    public bool increasing_b = false;


    private float zoomScaleX;

    private float scaleX;




    // Start is called before the first frame update
    void Start()
    {
        originalTransform = GetComponent<RectTransform>();
        originalPositionX = originalTransform.position.x;
        originalPositionY = originalTransform.position.y;

        originalScaleX = originalTransform.localScale.x;

        zoomScaleX = zoomedMinimapTransform.localScale.x;
        scaleX = zoomScaleX;
    }

    // Update is called once per frame
    void Update()
    {

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
        transform.position = Vector3.SmoothDamp(transform.position, zoomedMinimapTransform.position, ref zoomSpeed_vector, zoomTime, zoomSpeed);

        scaleX = Mathf.SmoothDamp(scaleX, zoomScaleX, ref zoomSpeed_, zoomTime, increasingScaleZoomSpeed);
        this.transform.localScale = new Vector3(scaleX, scaleX, transform.localScale.z);
    }

    private void decreasing()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(originalPositionX, originalPositionY, transform.position.z), ref zoomSpeed_vector, zoomTime, zoomSpeed);

        scaleX = Mathf.SmoothDamp(scaleX, originalScaleX, ref zoomSpeed_, zoomTime, zoomSpeed + 50);
        this.transform.localScale = new Vector3(scaleX, scaleX, transform.localScale.z);
    }
}
