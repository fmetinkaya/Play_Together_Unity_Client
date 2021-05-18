using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerScore
{
    public string matchID;
    public Player player;
    public int gameNo;
    public int score;

    public PlayerScore(int score, int gameNo, Player player, string matchID)
    {
        this.score = score;
        this.gameNo = gameNo;
        this.player = player;
        this.matchID = matchID;
    }
}
