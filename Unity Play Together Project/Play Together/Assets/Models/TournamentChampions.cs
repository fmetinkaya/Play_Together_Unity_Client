using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TournamentChampions
{
    public List<TournamentChampion> tournamentChampions;
    public TournamentChampions(List<TournamentChampion> tournamentChampions)
    {
        this.tournamentChampions = tournamentChampions;
    }

}
