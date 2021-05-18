using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class FriendshipManager : MonoBehaviour
{
    public GameObject friendNameButtonPrefab;
    public GameObject friendRequestCanvasPrefab;
    public GameObject friendsCanvasPrefab;
    GameObject friendsCanvas;
    public GameObject addFriendCanvasPrefab;
    GameObject addFriendCanvas;
    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;
    RoomManager roomManagerScript;
    Players friends;
    void Start()
    {
        socketClientManagerScript = GetComponent<SocketClientManager>();
        dialogueManagerScript = GetComponent<DialogueManager>();
        roomManagerScript = GetComponent<RoomManager>();
        socketClientManagerScript.friendshipRequestEvent += friendshipRequestListener;
        socketClientManagerScript.friendUpdateEvent += friendUpdateListener;

    }

    void friendUpdateListener(Socket socket, Packet originalPacket, params object[] args)
    {
        dialogueManagerScript.destroyWaitingCanvas();
        if (friendsCanvas)
        {
            getFriends();
        }
    }
    void friendshipRequestListener(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("friendshipRequest " + args[0].ToString() + args[1].ToString() + args[2].ToString());
        string requestOwnerName = args[0].ToString();
        string requestOwnerGlobalID = args[1].ToString();
        string requestedGlobalID = args[2].ToString();

        displayFriendRequestCanvas(requestOwnerName, requestOwnerGlobalID, requestedGlobalID);
    }

    void displayFriendRequestCanvas(string requestOwnerName, string requestOwnerGlobalID, string requestedGlobalID)
    {

        GameObject friendRequestCanvas = Instantiate(friendRequestCanvasPrefab, friendRequestCanvasPrefab.transform.position, Quaternion.identity);

        Button acceptButton = friendRequestCanvas.transform.GetChild(0).GetChild(1).GetChild(6).GetComponent<Button>();
        acceptButton.onClick.AddListener(delegate ()
        {
            Debug.Log("AcceptFriend Sent");
            socketClientManagerScript.manager.Socket.Emit("AcceptFriend", requestOwnerGlobalID, requestedGlobalID);
            Destroy(friendRequestCanvas);
        });

        Button rejectButton = friendRequestCanvas.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<Button>();
        rejectButton.onClick.AddListener(delegate () { Destroy(friendRequestCanvas); });

        Button closeButton = friendRequestCanvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate () { Destroy(friendRequestCanvas); });

        TextMeshProUGUI titleGUI = friendRequestCanvas.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
        titleGUI.text = "Friendship Request";

        TextMeshProUGUI questionGUI = friendRequestCanvas.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>();
        questionGUI.text = requestOwnerName + " Wants To Be Friends.";
    }
    public void getFriends()
    {
        Debug.Log("GetFriends Emit");
        dialogueManagerScript.displayWaitingCanvas("getFriends");
        socketClientManagerScript.manager.Socket.Emit("GetFriends", getFriendsCallBack, socketClientManagerScript.MyPlayer.playerGlobalID);
    }
    void getFriendsCallBack(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("getFriendsCallBack " + args[0].ToString());
        dialogueManagerScript.destroyWaitingCanvas();
        friends = JsonUtility.FromJson<Players>(args[0].ToString());
        friendsCanvasDisplay(friends);
    }
    void friendsCanvasDisplay(Players friends)
    {
        if (friendsCanvas)
        {
            Destroy(friendsCanvas);
        }
        friendsCanvas = Instantiate(friendsCanvasPrefab, friendsCanvasPrefab.transform.position, Quaternion.identity);
        Button exitButton = friendsCanvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();
        exitButton.onClick.AddListener(delegate () { Destroy(friendsCanvas); });

        Button addFriendButton = friendsCanvas.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Button>();
        addFriendButton.onClick.AddListener(delegate () { clickedOnAddFriend(); });

        friendsCanvas.transform.GetChild(0).GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = "My Id: " + socketClientManagerScript.MyPlayer.playerGlobalID;
        foreach (Player friend in friends.players)
        {
            GameObject friendNameButtonObject = Instantiate(friendNameButtonPrefab, friendNameButtonPrefab.transform.position, Quaternion.identity);
            friendNameButtonObject.transform.SetParent(friendsCanvas.transform.GetChild(0).GetChild(1).GetChild(5).GetChild(0), false);

            friendNameButtonObject.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = new AvatarsScript().GetAvatar(friend.playerAvatar);

            Button friendNameButton = friendNameButtonObject.transform.GetChild(3).GetComponent<Button>();
            friendNameButton.onClick.AddListener(delegate () { clickedOnFriend(friend); });

            friendNameButtonObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = friend.playerName;

            if (friend.playerSocketID != "")
            {
                friendNameButtonObject.transform.GetChild(3).gameObject.SetActive(true);
            }

            Button removeFriendButton = friendNameButtonObject.transform.GetChild(4).GetComponent<Button>();
            removeFriendButton.onClick.AddListener(delegate ()
            {
                RemoveFriendRequestDisplay(friend);
            });
        }
    }

    void RemoveFriendRequestDisplay(Player player)
    {
        Debug.Log("RemoveFriendRequestDisplay " + player.playerName);
        Action<bool, object[]> callbackFunction = RemoveFriend;
        object[] staticObjects = new object[] { player.playerGlobalID, socketClientManagerScript.MyPlayer.playerGlobalID };
        dialogueManagerScript.displayRequestCanvas("Get out of Friendship", "Do you want to get out of friendship with " + player.playerName + " ?", callbackFunction, staticObjects);
    }
    void RemoveFriend(bool isAccept, object[] staticObjects)
    {
        Debug.Log("RemoveFriend playerGlobalID" + staticObjects[0]);
        if (isAccept)
        {
            dialogueManagerScript.displayWaitingCanvas("RemoveFriend");
            socketClientManagerScript.manager.Socket.Emit("RemoveFriend", staticObjects[0].ToString(), staticObjects[1].ToString());
        }
    }
    void clickedOnFriend(Player friend)
    {
        Debug.Log("clickedOnFriend " + friend.playerName);
        roomManagerScript.inviteRoomRequest(friend.playerSocketID);
    }
    void clickedOnAddFriend()
    {
        Debug.Log("clickedOnAddFriend ");
        addFriendCanvasDisplay();
    }
    void addFriendCanvasDisplay()
    {
        addFriendCanvas = Instantiate(addFriendCanvasPrefab, addFriendCanvasPrefab.transform.position, Quaternion.identity);
        addFriendCanvas.transform.GetChild(0).GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = "My ID: " + socketClientManagerScript.MyPlayer.playerGlobalID;

        Button exitButton = addFriendCanvas.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>();
        exitButton.onClick.AddListener(delegate () { Destroy(addFriendCanvas); });

        TMP_InputField friendIDGUI = addFriendCanvas.transform.GetChild(0).GetChild(1).GetChild(6).GetComponent<TMP_InputField>();

        Button addFriendButton = addFriendCanvas.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<Button>();
        addFriendButton.onClick.AddListener(delegate ()
        {
            AddFriendRequestSend(friendIDGUI.text);
        });

    }

    public void AddFriendRequestSend(string playerGlobalID)
    {
        Debug.Log("AddFriendRequest Send");
        dialogueManagerScript.displayWaitingCanvas("AddFriendRequestSend");
        socketClientManagerScript.manager.Socket.Emit("AddFriendRequest", addFriendCallBack, socketClientManagerScript.MyPlayer.playerGlobalID, playerGlobalID);
    }
    void addFriendCallBack(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("AddFriend Request Callback");
        dialogueManagerScript.destroyWaitingCanvas();
        Debug.Log(args[0].ToString());
        if (Convert.ToInt32(args[0]) == 0)
        {
            //dialogueManagerScript.displayAlertCanvas("Operation Success", "Friendship Request Sent. Please refresh the page after your friend accepts your invitation.");
        }
        if (Convert.ToInt32(args[0]) == 1)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "User Not Found");
        }
        if (Convert.ToInt32(args[0]) == 2)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Users Are Already Friends");
        }
        if (Convert.ToInt32(args[0]) == 3)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Requested Friend is Not Online");
        }
        if (Convert.ToInt32(args[0]) == 4)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "You Cannot Be Friends With Yourself");
        }
        if (Convert.ToInt32(args[0]) == 5)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "ID Cannot Be Empty");
        }
    }
}
