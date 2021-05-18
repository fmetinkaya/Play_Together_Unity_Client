using System.Collections.Generic;
[System.Serializable]
public class Room
{
    public string roomName;
    public List<Player> players;
    public string roomID;
    public bool roomIsLookingForOppenent;
    public string roomMatchID;
    public TournamentRoom tournamentRoom;


    public Room(string roomName, List<Player> players, string roomID, bool roomIsLookingForOppenent, string roomMatchID, TournamentRoom tournamentRoom)
    {
        this.roomName = roomName;
        this.players = players;
        this.roomID = roomID;
        this.roomIsLookingForOppenent = roomIsLookingForOppenent;
        this.roomMatchID = roomMatchID;
        this.tournamentRoom = tournamentRoom;
    }


}

