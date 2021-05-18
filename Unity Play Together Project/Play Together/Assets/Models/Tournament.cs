using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tournament
{
    public List<TournamentGame> tournamentGames;
    public string tournamentDate;
    public string tournamentID;
    public string nextGameDate;
    public Tournament(List<TournamentGame> tournamentGames, string tournamentDate, string tournamentID, string nextGameDate)
    {
        this.tournamentGames = tournamentGames;
        this.tournamentDate = tournamentDate;
        this.tournamentID = tournamentID;
        this.nextGameDate = nextGameDate;
    }
}

