using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using UnityEngine.SceneManagement;


public class SocketClientManager : MonoBehaviour
{
    ScreenManager screenManager;
    private Player myPlayer;
    private static SocketClientManager obje = null;
    private bool isConnect = false;
    public SocketManager manager;

    public Action<Socket, Packet, object[]> connectEvent;
    public Action<Socket, Packet, object[]> disconnectEvent;
    public Action<Socket, Packet, object[]> updateRoomEvent;
    public Action<Socket, Packet, object[]> friendshipRequestEvent;
    public Action<Socket, Packet, object[]> inviteRoomRequestedEvent;
    public Action<Socket, Packet, object[]> startMatchEvent;
    public Action<Socket, Packet, object[]> scoreResultEvent;
    public Action<Socket, Packet, object[]> playerUpdateEvent;
    public Action<Socket, Packet, object[]> friendUpdateEvent;
    public Action<Socket, Packet, object[]> receiveMessageEvent;
    public Action<Socket, Packet, object[]> nextTournamentUpdateEvent;

    public bool IsConnect { get => isConnect; set => isConnect = value; }
    public Player MyPlayer { get => myPlayer; set => myPlayer = value; }

    void Start()
    {
        screenManager = gameObject.GetComponent<ScreenManager>();

        connectEvent += connectListener;
        disconnectEvent += disconnectListener;
        playerUpdateEvent += playerUpdateListener;

        SocketOptions options = new SocketOptions();
        options.ServerVersion = SupportedSocketIOVersions.v3;
        options.AutoConnect = true;

        manager = new SocketManager(new Uri("https://metinsocket.herokuapp.com/socket.io/"), options);
        Debug.Log("Socket Start");

        manager.Socket.On("connecting", (s, p, a) =>
        {
            Debug.Log("connecting");
        });
        manager.Socket.On("reconnect", (s, p, a) =>
        {
            Debug.Log("reconnect");
        });
        manager.Socket.On("reconnecting", (s, p, a) =>
        {
            Debug.Log("reconnecting");
        });

        manager.Socket.On(SocketIOEventTypes.Connect, (s, p, a) =>
            {
                connectEvent(s, p, a);
            });

        manager.Socket.On(SocketIOEventTypes.Disconnect, (s, p, a) =>
        {
            disconnectEvent(s, p, a);
        });

        manager.Socket.On("updateRoom", (s, p, a) =>
        {
            updateRoomEvent(s, p, a);
        });

        manager.Socket.On("FriendshipRequest", (s, p, a) =>
        {
            friendshipRequestEvent(s, p, a);
        });

        manager.Socket.On("InviteRoomRequested", (s, p, a) =>
        {
            inviteRoomRequestedEvent(s, p, a);
        });
        manager.Socket.On("StartMatch", (s, p, a) =>
        {
            startMatchEvent(s, p, a);
        });
        manager.Socket.On("ScoreResult", (s, p, a) =>
       {
           scoreResultEvent(s, p, a);
       });
        manager.Socket.On("PlayerUpdate", (s, p, a) =>
       {
           playerUpdateEvent(s, p, a);
       });

        manager.Socket.On("FriendUpdated", (s, p, a) =>
       {
           friendUpdateEvent(s, p, a);
       });

        manager.Socket.On("ReceiveMessage", (s, p, a) =>
       {
           receiveMessageEvent(s, p, a);
       });

        manager.Socket.On("NextTournamentUpdate", (s, p, a) =>
       {
           nextTournamentUpdateEvent(s, p, a);
       });

    }
    void connectListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("Connect");
        isConnect = true;
        screenManager.LoadScene(ScreenManager.Scene.LoginScreen);
    }
    void disconnectListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("Disconnect");
        isConnect = false;
        screenManager.LoadScene(ScreenManager.Scene.InitialScreen);
    }
    void playerUpdateListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("playerUpdateListener " + a[0].ToString());
        MyPlayer = JsonUtility.FromJson<Player>(a[0].ToString());
    }
    void Awake()
    {
        if (obje == null)
        {
            obje = this;
            DontDestroyOnLoad(this);
        }
        else if (this != obje)
        {
            Destroy(gameObject);
        }
    }
}
