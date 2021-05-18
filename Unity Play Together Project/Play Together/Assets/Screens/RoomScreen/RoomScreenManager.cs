using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class RoomScreenManager : MonoBehaviour
{
    public GameObject PlayTogetherSelect;
    public GameObject tournamentStats;
    public GameObject tournamentButtons;
    public GameObject championsButton;
    public GameObject tournamentRoomBottom;
    public GameObject normalRoomBottom;
    public GameObject tournamentTimeJoinButton;
    public GameObject tournamentCloseButton;
    public RectTransform bottomButtons;
    public RectTransform playersContainer;

    public GameObject playerButtonMePrefab;
    public GameObject playerListContent;
    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;
    RoomManager roomManagerScript;
    FriendshipManager friendshipManagerScript;
    TournamentManager tournamentManagerScript;
    public Button friendsButton;
    public Button lookForOppenentButton;
    public Button playTogetherButton;

    public RectTransform searchIcon;
    public GameObject searchIconFocus;


    public TextMeshProUGUI roomName;

    public GameObject playerInfoCanvasPrefab;

    bool amIRoomManager = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        dialogueManagerScript = gameManager.GetComponent<DialogueManager>();
        roomManagerScript = gameManager.GetComponent<RoomManager>();
        friendshipManagerScript = gameManager.GetComponent<FriendshipManager>();
        tournamentManagerScript = gameManager.GetComponent<TournamentManager>();

        socketClientManagerScript.updateRoomEvent += updateRoomListener;

        roomGuiUpdate(roomManagerScript.Room);

        friendsButton.onClick.AddListener(delegate ()
         {
             friendshipManagerScript.getFriends();
         });

        lookForOppenentButton.onClick.AddListener(delegate ()
        {
            Debug.Log("Sent lookForOppenent ");
            dialogueManagerScript.displayWaitingCanvas("lookForOppenentButton");
            socketClientManagerScript.manager.Socket.Emit("LookForMatch", !roomManagerScript.Room.roomIsLookingForOppenent);
        });
        playTogetherButton.onClick.AddListener(delegate ()
        {
            Debug.Log("Sent playTogether ");
            PlayTogetherSelect.SetActive(true);
        });


        tournamentTimeJoinButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            tournamentManagerScript.JoinTournamentRoom();
        });
        tournamentCloseButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            tournamentManagerScript.LeaveTournamentRoom();
        });
        championsButton.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            tournamentManagerScript.GetTournamentChampions();
        });

        SearchIconAnimation();

    }
    public void playTogether(int gameNo)
    {
        dialogueManagerScript.displayWaitingCanvas("playTogetherButton");
        socketClientManagerScript.manager.Socket.Emit("PlayTogether", gameNo);
    }
    void Update()
    {
        if (tournamentManagerScript.nextTournament != null)
        {
            string tournamentTimeString = getTimeSpan(Convert.ToDateTime(tournamentManagerScript.nextTournament.tournamentDate));
            tournamentTimeJoinButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Tournament: <size=+5><color=red>" + tournamentTimeString + "</color></size> ";
        }
        else
        {
            tournamentTimeJoinButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Current Tournaments";
        }

        if (roomManagerScript.Room.tournamentRoom.roomID != null)
        {
            string nextGameDateString = getTimeSpan(Convert.ToDateTime(roomManagerScript.Room.tournamentRoom.tournament.nextGameDate));
            tournamentStats.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Next Game Time: <size=+5><color=red>" + nextGameDateString + "</color></size>";
        }


    }
    void updateRoomListener(Socket s, Packet p, object[] a)
    {
        dialogueManagerScript.destroyWaitingCanvas();
        Debug.Log("updateRoomGUIProcess " + a[0].ToString());
        roomGuiUpdate(roomManagerScript.Room);
    }

    void SearchIconAnimation()
    {
        searchIcon.DOAnchorPos(new Vector2(UnityEngine.Random.Range(-150f, 150), UnityEngine.Random.Range(-80, 80)), 1).OnComplete(() => { SearchIconAnimation(); });
    }

    public void roomGuiUpdate(Room room)
    {
        Debug.Log("roomGuiUpdate ");

        amIRoomManager = AuthorizationGUI(room);

        isOnTournamentGUIUpdate(room.tournamentRoom);

        tournamentStatsUpdate(room);

        SearchAnimationUpdate(room);

        roomName.text = room.roomName;

        RoomPlayerUpdate(room);

    }
    void tournamentStatsUpdate(Room room)
    {
        if (room.tournamentRoom.roomID != null)
        {
            string playedGamesString = room.tournamentRoom.playedGames + "/" + room.tournamentRoom.tournament.tournamentGames.Count;
            string wonGamesString = room.tournamentRoom.wonGames + "/" + room.tournamentRoom.playedGames;

            tournamentStats.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Played Game:  <size=+5><color=red>" + playedGamesString + "</color></size>";
            tournamentStats.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Won Game:   <size=+5><color=red>" + wonGamesString + "</color></size>";
        }
    }

    void SearchAnimationUpdate(Room room)
    {
        searchIcon.gameObject.SetActive(room.roomIsLookingForOppenent);
        searchIconFocus.SetActive(room.roomIsLookingForOppenent);

    }

    void RoomPlayerUpdate(Room room)
    {
        foreach (Transform child in playerListContent.transform)
        {
            Destroy(child.gameObject);
        }
        int i = 1;
        foreach (Player player in room.players)
        {
            GameObject playerListItem = Instantiate(playerButtonMePrefab, playerButtonMePrefab.transform.position, Quaternion.identity);
            if (player.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
            {
                playerListItem.transform.GetChild(1).GetComponent<Image>().enabled = true;
            }

            Button playerButton = playerListItem.GetComponent<Button>();
            playerButton.onClick.AddListener(delegate ()
           {
               playerButtonOnClick(player);
           });

            if (player.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID || amIRoomManager)
            {
                playerListItem.transform.GetChild(4).gameObject.SetActive(true);

            }

            Button kickPlayerButton = playerListItem.transform.GetChild(4).GetComponent<Button>();
            kickPlayerButton.onClick.AddListener(delegate ()
           {
               if (player.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
               {
                   LeaveFromRoomRequestDisplay();
               }
               else
               {
                   KickFromRoomRequestDisplay(player);
               }
           });

            playerListItem.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = new AvatarsScript().GetAvatar(player.playerAvatar);

            playerListItem.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = i++.ToString();

            playerListItem.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = player.playerName;

            playerListItem.transform.SetParent(playerListContent.transform, false);

        }
    }
    bool AuthorizationGUI(Room room)
    {
        if (room.players[0].playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
        {
            lookForOppenentButton.interactable = true;
            playTogetherButton.interactable = true;
            return true;
        }
        else
        {
            lookForOppenentButton.interactable = false;
            playTogetherButton.interactable = false;
            return false;
        }
    }
    void playerButtonOnClick(Player player)
    {
        Debug.Log("Click " + player.playerName);
        GameObject playerInfoPopup = Instantiate(playerInfoCanvasPrefab);
        playerInfoPopup.GetComponent<PlayerInfoScript>().player = player;
    }
    void KickFromRoomRequestDisplay(Player player)
    {
        Debug.Log("KickFromRoomRequestDisplay " + player.playerName);
        Action<bool, object[]> callbackFunction = KickFromRoom;
        object[] staticObjects = new object[] { player.playerSocketID, roomManagerScript.Room.roomID };
        dialogueManagerScript.displayRequestCanvas("Kick From The Room", "Do you want to kick " + player.playerName + " from the room ?", callbackFunction, staticObjects);
    }
    void KickFromRoom(bool isAccept, object[] staticObjects)
    {
        Debug.Log("KickFromRoom playerSocketID" + staticObjects[0]);
        if (isAccept)
        {
            dialogueManagerScript.displayWaitingCanvas("KickFromRoom");
            socketClientManagerScript.manager.Socket.Emit("KickFromRoom", staticObjects[0].ToString(), staticObjects[1].ToString());
        }
    }

    void LeaveFromRoomRequestDisplay()
    {
        Debug.Log("LeaveFromRoomRequestDisplay");
        Action<bool, object[]> callbackFunction = LeaveFromRoom;
        object[] staticObjects = new object[] { };
        dialogueManagerScript.displayRequestCanvas("Leave From The Room", "Do you want to leave the room ?", callbackFunction, staticObjects);
    }
    void LeaveFromRoom(bool isAccept, object[] staticObjects)
    {
        Debug.Log("LeaveFromRoom");
        if (isAccept)
        {
            dialogueManagerScript.displayWaitingCanvas("LeaveFromRoom");
            socketClientManagerScript.manager.Socket.Emit("LeaveFromRoom");
        }
    }
    void isOnTournamentGUIUpdate(TournamentRoom tournamentRoom)
    {
        Debug.Log("isOnTournamentGUIUpdate " + tournamentRoom.roomID);

        bool isOnTournament = false;
        if (tournamentRoom.roomID != null)
        {
            isOnTournament = true;
        }

        Debug.Log("isOnTournamentGUIUpdate " + isOnTournament);
        tournamentRoomBottom.SetActive(isOnTournament);
        normalRoomBottom.SetActive(!isOnTournament);
        tournamentButtons.SetActive(!isOnTournament);
        tournamentCloseButton.SetActive(isOnTournament);
        if (isOnTournament)
        {
            playersContainer.offsetMin = new Vector2(playersContainer.offsetMin.x, 300);
            bottomButtons.sizeDelta = new Vector2(bottomButtons.sizeDelta.x, 230);
        }
        else
        {
            playersContainer.offsetMin = new Vector2(playersContainer.offsetMin.x, 420);
            bottomButtons.sizeDelta = new Vector2(bottomButtons.sizeDelta.x, 350);
        }



    }

    string getTimeSpan(DateTime startDate)
    {
        TimeSpan timeSpan = startDate - DateTime.Now;
        string timeSpanString;


        timeSpanString = startDate.ToString("dd/MM HH:mm");

        if (timeSpan.Days == 0)
        {
            timeSpanString = timeSpan.ToString(@"hh\:mm\:ss");
            if (timeSpan.Hours == 0)
            {
                timeSpanString = timeSpan.ToString(@"mm\:ss");
                if (timeSpan.Minutes == 0)
                {
                    timeSpanString = timeSpan.ToString(@"ss");
                }
            }
        }
        return timeSpanString;
    }
    void OnDestroy()
    {
        print("RoomScreenManager was destroyed");
        socketClientManagerScript.updateRoomEvent -= updateRoomListener;
    }

}

