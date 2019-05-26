﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private int pointsToWin;

    private void Awake()
    {
        instance = this;
    }

    private bool Victory()
    {
        bool b = false;

        if (playerManager.PlayerScore >= pointsToWin)
            b = true;

        return b;
    }

    public void EndGame()
    {
        if (Victory())
            Debug.Log("YOU WIN!!!!");
        else
            Debug.Log("YOU LOSE!");
    }
}
