using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerGameNoScore
{
    public int gameNo;
    public int playedGamesNumber;
    public float avarageScore;
    public int highScore;
    public string gameName;

    public PlayerGameNoScore(int gameNo, int playedGamesNumber, float avarageScore, int highScore, string gameName)
    {
        this.gameNo = gameNo;
        this.playedGamesNumber = playedGamesNumber;
        this.avarageScore = avarageScore;
        this.highScore = highScore;
        this.gameName = gameName;
    }

}
