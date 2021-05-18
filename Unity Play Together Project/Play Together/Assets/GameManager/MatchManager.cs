using UnityEngine;
using System;
using BestHTTP.SocketIO;

public class MatchManager : MonoBehaviour
{
    ScreenManager screenManager;
    SocketClientManager socketClientManagerScript;
    Match match;
    public Match Match { get => match; set => match = value; }

    void Start()
    {
        socketClientManagerScript = GetComponent<SocketClientManager>();
        screenManager = gameObject.GetComponent<ScreenManager>();

        socketClientManagerScript.startMatchEvent += startMatchListener;
    }
    void startMatchListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("startMatchListener " + a[0].ToString());

        Match = JsonUtility.FromJson<Match>(a[0].ToString());

        Match.startMatchTime = DateTime.Now;

        screenManager.LoadGameScene(Match.game.gameNo);
    }

}
