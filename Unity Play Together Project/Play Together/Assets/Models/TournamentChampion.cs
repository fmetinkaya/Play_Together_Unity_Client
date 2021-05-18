using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TournamentChampion
{
    public List<TCRoom> TCRooms;
    public int playedGames;
    public string tournamentDate;
    public TournamentChampion(List<TCRoom> TCRooms, int playedGames, string tournamentDate)
    {
        this.TCRooms = TCRooms;
        this.playedGames = playedGames;
        this.tournamentDate = tournamentDate;
    }

}
