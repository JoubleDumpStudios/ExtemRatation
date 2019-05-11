using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Plant_Behaviour : MonoBehaviour
{

    SpriteRenderer spriteRenderer;
    bool attacked;
    float time;

    [SerializeField]
    private float redTimeForPlant;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacked)
            AttackTimer();
    }

    public void PlantAttacked()
    {
        spriteRenderer.color = Color.red;
        attacked = true;
    }

    private void AttackTimer()
    {
        time += Time.deltaTime;
        if (time > redTimeForPlant)
        {
            spriteRenderer.color = Color.white;
            time = 0;
            attacked = false;
        }
    }

}
