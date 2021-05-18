using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO;

public class ChatManager : MonoBehaviour
{
    public List<ChatMessage> chatMessages;
    SocketClientManager socketClientManagerScript;

    RoomManager roomManagerScript;
    void Start()
    {
        socketClientManagerScript = gameObject.GetComponent<SocketClientManager>();
        roomManagerScript = gameObject.GetComponent<RoomManager>();

        roomManagerScript.roomChangedEvent += roomChangedListener;
        socketClientManagerScript.receiveMessageEvent += ReceiveMessageListener;

        chatMessages = new List<ChatMessage>();

    }
    void ReceiveMessageListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("ReceiveMessageListener " + a[0].ToString());
        ChatMessage chatMessage = JsonUtility.FromJson<ChatMessage>(a[0].ToString());
        chatMessages.Add(chatMessage);
    }
    void roomChangedListener(string previousRoomID, string newRoomID)
    {
        Debug.Log("roomChangedListener previous roomID" + previousRoomID + " new roomID " + newRoomID);
        chatMessages.Clear();
    }

}
