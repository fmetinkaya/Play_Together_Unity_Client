using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TournamentRoom
{
    public string roomID;
    public int wonGames;
    public int playedGames;
    public Tournament tournament;
    public TournamentRoom(string roomID, int wonGames, int playedGames, Tournament tournament)
    {
        this.roomID = roomID;
        this.wonGames = wonGames;
        this.playedGames = playedGames;
        this.tournament = tournament;
    }
}
