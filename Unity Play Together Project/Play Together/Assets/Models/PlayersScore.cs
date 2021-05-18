using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayersScore
{
    public List<PlayerScore> playersScore;
    public PlayersScore(List<PlayerScore> playersScore)
    {
        this.playersScore = playersScore;
    }
}
