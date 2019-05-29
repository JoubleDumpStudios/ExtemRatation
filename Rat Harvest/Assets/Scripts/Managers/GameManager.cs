using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private int pointsToWin;
    public int PointsToWin { get { return this.pointsToWin; } }

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
            SceneManager.instance.loadYouWin();
        else
            SceneManager.instance.loadYouLose();
    }
}
