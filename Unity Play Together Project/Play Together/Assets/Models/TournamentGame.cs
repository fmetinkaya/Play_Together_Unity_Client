using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TournamentGame
{
    public int gameNo;
    public int waitAtRoomScreen;
    public TournamentGame(int gameNo, int waitAtRoomScreen)
    {
        this.gameNo = gameNo;
        this.waitAtRoomScreen = waitAtRoomScreen;
    }
}
