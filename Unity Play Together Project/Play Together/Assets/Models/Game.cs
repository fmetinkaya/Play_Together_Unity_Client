using System.Collections.Generic;

[System.Serializable]

public class Game
{
    public int gameNo;
    public string gameName;
    public int gameScreenGameTime;
    public Game(int gameNo, string gameName, int gameScreenGameTime)
    {
        this.gameNo = gameNo;
        this.gameName = gameName;
        this.gameScreenGameTime = gameScreenGameTime;

    }
}
