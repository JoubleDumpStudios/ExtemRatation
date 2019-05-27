﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Health_Logic : MonoBehaviour
{

    public RectTransform topHeadHealthBar;

    private Rat_Movement_Logic ratMovementScript;

    [SerializeField]
    private List<GameObject> wounds_ = new List<GameObject>();

    public float maxHealth = 100;
    public float health;
    // Start is called before the first frame update

    public void OnEnable()
    {
        ResetRatHealth();
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
    }


    public void Start()
    {
        ResetRatHealth();
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
        ratMovementScript = GetComponent<Rat_Movement_Logic>();
    }

    public void ratHited(float damage/*, GameObject wound*/)
    {
        AkSoundEngine.PostEvent("Rat_Hit", gameObject);
        if (health >= 0)
        { 
            health -= damage;
        }
        else
        {
            AkSoundEngine.PostEvent("Rat_Death", gameObject);
            health = 0;
            ratMovementScript.killRat();
            //cleanWounds(wounds_);
        }

        /*wounds_.Add(wound);*/ // each time the rat recieve damage this method recieve the object that allow us to visualice the hole and add it to a list to manage it
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
    }


    void cleanWounds(List<GameObject> wounds)// allow us to remove the wounds from the rat and send them again to the pooler to be used again when needed
    {
        for (int i = 0; i < wounds.Count; i++)
        {
            ObjectPooler.instance.killGameObject(wounds[i]);
            wounds[i].transform.parent = null;
        }

        wounds.Clear();
    }

    public void ResetRatHealth()
    {
        health = maxHealth;
    }
}
