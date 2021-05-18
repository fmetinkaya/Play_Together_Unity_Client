using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;


public class TournamentManager : MonoBehaviour
{
    public Tournament nextTournament;

    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;

    public GameObject TournamentChampionsCanvasPrefab;


    void Start()
    {
        socketClientManagerScript = gameObject.GetComponent<SocketClientManager>();
        dialogueManagerScript = gameObject.GetComponent<DialogueManager>();

        socketClientManagerScript.nextTournamentUpdateEvent += nextTournamentUpdateListener;

    }
    public void nextTournamentUpdateListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("nextTournamentUpdateListener" + a[0].ToString());
        if (a[0].ToString() != "null")
        {
            nextTournament = JsonUtility.FromJson<Tournament>(a[0].ToString());
        }
        else
        {
            nextTournament = JsonUtility.FromJson<Tournament>(null);
        }

    }
    public void JoinTournamentRoom()
    {
        Debug.Log("JoinTournamentRoom ");
        dialogueManagerScript.displayWaitingCanvas("JoinTournamentRoom");
        socketClientManagerScript.manager.Socket.Emit("JoinTournamentRoom");
    }
    public void LeaveTournamentRoom()
    {
        Debug.Log("LeaveTournamentRoom ");
        dialogueManagerScript.displayWaitingCanvas("LeaveTournamentRoom");
        socketClientManagerScript.manager.Socket.Emit("LeaveTournamentRoom");
    }

    public void GetTournamentChampions()
    {
        Debug.Log("GetTournamentChampions Sent");
        dialogueManagerScript.displayWaitingCanvas("GetTournamentChampions");
        socketClientManagerScript.manager.Socket.Emit("GetTournamentChampions", GetTournamentChampionsCallBack);
    }
    void GetTournamentChampionsCallBack(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("GetTournamentChampionsCallBack " + args[0].ToString());
        dialogueManagerScript.destroyWaitingCanvas();
        TournamentChampions tournamentChampions = JsonUtility.FromJson<TournamentChampions>(args[0].ToString());

        GameObject tournamentChampionsCanvas = Instantiate(TournamentChampionsCanvasPrefab);
        TournamentChampionsScript tournamentChampionsScript = tournamentChampionsCanvas.GetComponent<TournamentChampionsScript>();

        tournamentChampionsScript.tournamentChampions = tournamentChampions;
    }

}
