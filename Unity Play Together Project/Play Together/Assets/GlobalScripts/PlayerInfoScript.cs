using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerInfoScript : MonoBehaviour
{
    public Player player;

    public TextMeshProUGUI playerNameGUI;
    public Image playerAvatarGUI;

    public TextMeshProUGUI playerMainScoreGUI;
    public TextMeshProUGUI playerWeeklyScoreGUI;
    public TextMeshProUGUI totalWinRateGUI;

    public TextMeshProUGUI playerMatchWinsGUI;
    public TextMeshProUGUI matchWinRateGUI;
    public TextMeshProUGUI playerTotalMatchGamesGUI;


    public TextMeshProUGUI playerSoloWinsGUI;
    public TextMeshProUGUI soloWinRateGUI;
    public TextMeshProUGUI playerTotalSoloGamesGUI;


    public TextMeshProUGUI playerTournamentWinsGUI;
    public TextMeshProUGUI tournamentWinRateGUI;
    public TextMeshProUGUI playerTotalTournamentGamesGUI;

    public Button addFriendButton;

    GameObject gameManager;
    FriendshipManager friendshipManagerScript;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        friendshipManagerScript = gameManager.GetComponent<FriendshipManager>();

        /*List<PlayerGameNoScore> playerGameNoScore = new List<PlayerGameNoScore>();
        PlayerScores playerScores = new PlayerScores(1, 1, playerGameNoScore, 7, 8, 7, 8);
        player = new Player("denemePlayerName", "globalID", "socketID", 5, "playerRoomId", "personalRoomName", playerScores);*/

        addFriendButton.onClick.AddListener(delegate ()
        {
            AddFriendRequestSend(player.playerGlobalID);
        });

        playerInfoGUIUpdate();
    }
    public void playerInfoGUIUpdate()
    {
        playerNameGUI.text = player.playerName;
        playerAvatarGUI.sprite = new AvatarsScript().GetAvatar(player.playerAvatar);

        playerMainScoreGUI.text = player.playerScores.playerMainScore.ToString();
        playerWeeklyScoreGUI.text = player.playerScores.playerWeeklyScore.ToString();
        totalWinRateGUI.text = GetPercentageString(player.playerScores.playerSoloWins + player.playerScores.playerMatchWins + player.playerScores.playerTournamentWins, player.playerScores.playerTotalSoloGames + player.playerScores.playerTotalMatchGames + player.playerScores.playerTotalTournamentGames);

        playerMatchWinsGUI.text = player.playerScores.playerMatchWins.ToString();
        matchWinRateGUI.text = GetPercentageString(player.playerScores.playerMatchWins, player.playerScores.playerTotalMatchGames);
        playerTotalMatchGamesGUI.text = player.playerScores.playerTotalMatchGames.ToString();

        playerSoloWinsGUI.text = player.playerScores.playerSoloWins.ToString();
        soloWinRateGUI.text = GetPercentageString(player.playerScores.playerSoloWins, player.playerScores.playerTotalSoloGames);
        playerTotalSoloGamesGUI.text = player.playerScores.playerTotalSoloGames.ToString();

        playerTournamentWinsGUI.text = player.playerScores.playerTournamentWins.ToString();
        tournamentWinRateGUI.text = GetPercentageString(player.playerScores.playerTournamentWins, player.playerScores.playerTotalTournamentGames);
        playerTotalTournamentGamesGUI.text = player.playerScores.playerTotalTournamentGames.ToString();
    }
    string GetPercentageString(int current, int maximum)
    {
        if (current == 0 || maximum == 0)
        {
            return 0.ToString("0.00%");
        }
        return ((float)current / (float)maximum).ToString("0.0%");
    }

    public void closePlayerInfoPopup()
    {
        Destroy(gameObject);
    }

    void AddFriendRequestSend(string playerGlobalID)
    {
        friendshipManagerScript.AddFriendRequestSend(playerGlobalID);
    }

}
