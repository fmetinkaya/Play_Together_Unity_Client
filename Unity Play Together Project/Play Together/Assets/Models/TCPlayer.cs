using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TCPlayer
{
    public string playerGlobalID;
    public int playerAvatar;
    public string playerName;
    public TCPlayer(string playerGlobalID, int playerAvatar, string playerName)
    {
        this.playerGlobalID = playerGlobalID;
        this.playerAvatar = playerAvatar;
        this.playerName = playerName;
    }
}
