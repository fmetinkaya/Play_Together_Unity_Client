using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TCRoom
{
    public List<TCPlayer> TCPlayers;
    public int wonGames;
    public string roomName;
    public TCRoom(List<TCPlayer> TCPlayers, string roomName, int wonGames)
    {
        this.TCPlayers = TCPlayers;
        this.roomName = roomName;
        this.wonGames = wonGames;
    }
}
