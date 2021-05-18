using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class RoomManager : MonoBehaviour
{
    private Room room;

    SocketClientManager socketClientManagerScript;

    DialogueManager dialogueManagerScript;

    public GameObject inviteRoomRequestedCanvasPrefab;

    public Room Room { get => room; set => room = value; }

    public Action<string, string> roomChangedEvent;

    void Start()
    {
        Debug.Log("Room Manager Start");
        socketClientManagerScript = GetComponent<SocketClientManager>();
        dialogueManagerScript = GetComponent<DialogueManager>();
        socketClientManagerScript.updateRoomEvent += updateRoomListener;
        socketClientManagerScript.inviteRoomRequestedEvent += inviteRoomRequestedListener;
    }

    public void inviteRoomRequest(string invitedPlayerSocketID)
    {
        if (invitedPlayerSocketID != "")
            socketClientManagerScript.manager.Socket.Emit("InviteRoomRequest", invitedPlayerSocketID);
    }
    void updateRoomListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("updateRoom " + a[0].ToString());

        room = JsonUtility.FromJson<Room>(a[0].ToString());
        if (socketClientManagerScript.MyPlayer != null && socketClientManagerScript.MyPlayer.playerRoomID != room.roomID)
        {
            roomChangedEvent(socketClientManagerScript.MyPlayer.playerRoomID, room.roomID);
            socketClientManagerScript.MyPlayer.playerRoomID = room.roomID;
        }
    }
    void inviteRoomRequestedListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("inviteRoomRequested " + a[0].ToString() + a[1].ToString() + a[2].ToString());
        displayInviteRoomRequestedCanvas(a[0].ToString(), a[1].ToString(), a[2].ToString());

    }

    void displayInviteRoomRequestedCanvas(string requestOwnerName, string requestOwnerRoomName, string requestOwnerRoomID)
    {
        Debug.Log("displayInviteRoomRequestedCanvas ");
        Action<bool, object[]> callbackFunction = AcceptInviteRoomRequestCallBack;
        object[] staticObjects = new object[] { requestOwnerRoomID };
        dialogueManagerScript.displayRequestCanvas("Room Invitation", requestOwnerName + " Invites to " + requestOwnerRoomName, callbackFunction, staticObjects);
    }

    void AcceptInviteRoomRequestCallBack(bool isAccept, object[] staticObjects)
    {
        Debug.Log("KickFromRoom playerSocketID" + staticObjects[0]);
        if (isAccept)
        {
            Debug.Log("AcceptInviteRoomRequest Sent");
            socketClientManagerScript.manager.Socket.Emit("AcceptInviteRoomRequest", staticObjects[0]);
        }
    }

}
