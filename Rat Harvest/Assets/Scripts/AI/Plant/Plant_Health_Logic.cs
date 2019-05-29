using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Health_Logic : MonoBehaviour
{
    [SerializeField] private RectTransform topHeadHealthBar;

    public void updateHealth(int health)
    {
        topHeadHealthBar.sizeDelta = new Vector2(health * 2, topHeadHealthBar.sizeDelta.y);
    }

}
