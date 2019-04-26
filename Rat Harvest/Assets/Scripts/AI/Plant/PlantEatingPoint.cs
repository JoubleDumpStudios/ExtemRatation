using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEatingPoint : MonoBehaviour
{
    public bool ratEatingMe = false;
    float time = 0;

    Rat_Movement_Logic rat;

    [SerializeField]
    private float eatingTime = 3;

    private bool hasRat = false;
    public bool HasRat { get { return this.hasRat; } set { this.hasRat = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (ratEatingMe )
        //{
        //    time += Time.deltaTime;
        //    if (time >= eatingTime)
        //    {
        //        ratEatingMe = false;
        //        rat.eatPlant(this.gameObject);
        //        rat.chooseNewTarget();
        //        time = 0;
        //    }
               
        //}
    }

    private void OnTriggerEnter(Collider other)//when the nav mesh agent arrives to this destination it takes the RatMovementLogic, it is necessary to call the methos to find another target for the nav mesh agent
    {
        rat = other.gameObject.GetComponent<Rat_Movement_Logic>();
        ratEatingMe = true;
    }

}
