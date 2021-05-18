using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Players
{
    public List<Player> players;
    public Players(List<Player> players)
    {
        this.players = players;
    }
}
