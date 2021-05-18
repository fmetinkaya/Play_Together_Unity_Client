using System.Collections.Generic;
using System;

[System.Serializable]

public class Match
{
    public int matchType;             //type == 1 (together), type == 2 (opponentRoom), type == 3 (tournament)
    public int gameScreenWaitTime;
    public DateTime startMatchTime;
    public string matchID;
    public Game game;

    public Match(string matchID, int matchType, DateTime startMatchTime, int gameScreenWaitTime, Game game)
    {
        this.matchID = matchID;
        this.game = game;
        this.gameScreenWaitTime = gameScreenWaitTime;
        this.startMatchTime = startMatchTime;
        this.matchType = matchType;
    }

}
