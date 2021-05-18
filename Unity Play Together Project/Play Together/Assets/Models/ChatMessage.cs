using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatMessage
{
    public int playerAvatar;
    public string playerName;
    public string playerGlobalID;
    public string playerRoomID;
    public string message;
    public string date;

    public ChatMessage(string message, string playerName, int playerAvatar, string playerGlobalID, string date, string playerRoomID)
    {
        this.message = message;
        this.playerName = playerName;
        this.playerAvatar = playerAvatar;
        this.playerGlobalID = playerGlobalID;
        this.date = date;
        this.playerRoomID = playerRoomID;
    }
}

