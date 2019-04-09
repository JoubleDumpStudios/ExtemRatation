using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Health_Logic : MonoBehaviour
{

    public RectTransform topHeadHealthBar;


    public float maxHealth = 100;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit(float damage)
    {
        if(health>=0)
            health -= damage;

        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);

    }

}
