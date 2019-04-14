using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Player's life
    [SerializeField] private int playerHealth = 100;

    // Player's score
    private int playerScore = 0;
    public int PlayerScore { get { return this.playerScore; } set { this.playerScore = value; } }
}
