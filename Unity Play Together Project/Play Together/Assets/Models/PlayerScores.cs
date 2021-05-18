using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerScores
{
    public int playerMainScore;
    public int playerWeeklyScore;
    public int playerSoloWins;
    public int playerTotalSoloGames;
    public int playerMatchWins;
    public int playerTotalMatchGames;
    public int playerTournamentWins;
    public int playerTotalTournamentGames;
    public List<PlayerGameNoScore> playerGamesNoScore;

    public PlayerScores(int playerMainScore, int playerWeeklyScore, List<PlayerGameNoScore> playerGamesNoScore, int playerSoloWins, int playerTotalSoloGames, int playerMatchWins, int playerTotalMatchGames, int playerTournamentWins, int playerTotalTournamentGames)
    {
        this.playerMainScore = playerMainScore;
        this.playerWeeklyScore = playerWeeklyScore;
        this.playerGamesNoScore = playerGamesNoScore;
        this.playerSoloWins = playerSoloWins;
        this.playerTotalSoloGames = playerTotalSoloGames;
        this.playerMatchWins = playerMatchWins;
        this.playerTotalMatchGames = playerTotalMatchGames;
        this.playerTournamentWins = playerTournamentWins;
        this.playerTotalTournamentGames = playerTotalTournamentGames;
    }
}
