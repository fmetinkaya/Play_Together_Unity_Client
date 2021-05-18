using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Settings
{
    public int playerAvatar;
    public string playerName;
    public string playerGlobalID;
    public string personalRoomName;


    public Settings(string playerName, int playerAvatar, string personalRoomName, string playerGlobalID)
    {
        this.playerAvatar = playerAvatar;
        this.playerName = playerName;
        this.playerGlobalID = playerGlobalID;
        this.personalRoomName = personalRoomName;
    }
}
