using System.Collections.Generic;

[System.Serializable]
public class Player
{
    public int playerAvatar;
    public string playerName;
    public string playerGlobalID;
    public string playerSocketID;
    public string playerRoomID;
    public string personalRoomName;
    public PlayerScores playerScores;


    public Player(string playerName, string playerGlobalID, string playerSocketID, int playerAvatar, string playerRoomID, string personalRoomName, PlayerScores playerScores)
    {
        this.playerSocketID = playerSocketID;
        this.playerGlobalID = playerGlobalID;
        this.playerName = playerName;
        this.playerScores = playerScores;
        this.playerAvatar = playerAvatar;
        this.playerRoomID = playerRoomID;
        this.personalRoomName = personalRoomName;

    }
}
