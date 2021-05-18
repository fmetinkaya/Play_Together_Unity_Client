using System.Collections;
using System;
using UnityEngine;
using BestHTTP.SocketIO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;



public class GameScreenManager : MonoBehaviour

{
    public GameObject finishConfettiEffectPrefab;
    public GameObject countDownGameTimeCanvas;
    public GameObject countDownWaitForStartPrefab;

    public GameObject playerInfoCanvasPrefab;
    public GameObject playersScoreCanvasPrefab;
    public GameObject playerScorePanelPrefab;
    GameObject playersScoreCanvas;
    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    MatchManager matchManagerScript;
    RoomManager roomManagerScript;
    ScreenManager screenManager;
    PlayerScore playerScore;

    Player previousPlayerStats;
    public Match match;

    PlayersScore playersScore;

    public Player PreviousPlayerStats { get => previousPlayerStats; set => previousPlayerStats = value; }

    TimeSpan timeDifferent;

    public _screenOrientation screenOrientation;

    PlayerScore playerScoreComingFromServer;

    public enum _screenOrientation
    {
        Landscape,
        Portrait
    }
    void Awake()
    {
        if (screenOrientation == _screenOrientation.Landscape)
            Screen.orientation = ScreenOrientation.Landscape;
        if (screenOrientation == _screenOrientation.Portrait)
            Screen.orientation = ScreenOrientation.Portrait;

        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        matchManagerScript = gameManager.GetComponent<MatchManager>();
        roomManagerScript = gameManager.GetComponent<RoomManager>();
        screenManager = gameManager.GetComponent<ScreenManager>();


        match = matchManagerScript.Match;

    }

    void Start()
    {
        Debug.Log("GameScreenManager was started");

        PreviousPlayerStats = socketClientManagerScript.MyPlayer;

        playerScore = new PlayerScore(0, match.game.gameNo, socketClientManagerScript.MyPlayer, match.matchID);

        socketClientManagerScript.scoreResultEvent += ScoreResultListener;

        StartCoroutine(WaitingForStartGame());

    }

    public RandomManager GetNewRandomManager()
    {
        return new RandomManager(Convert.ToInt32(match.matchID.Substring(match.matchID.Length - 6, 5)));
    }
    IEnumerator WaitingForStartGame()
    {
        float waitTime = match.gameScreenWaitTime - getDateDifferenceMillisecond(match.startMatchTime);
        Debug.Log("WaitingForStartGame " + waitTime);
        displayCountDownWaitForStart(waitTime);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(waitTime / 1000f);
        Time.timeScale = 1;
        StartCoroutine(SendPlayerScore());
    }
    IEnumerator SendPlayerScore()
    {
        Debug.Log("SendPlayerScore " + getDateDifferenceMillisecond(match.startMatchTime));
        displayCountDownGameTime((float)match.game.gameScreenGameTime / 1000);
        yield return new WaitForSecondsRealtime(match.game.gameScreenGameTime / 1000f);
        Debug.Log("SendPlayerScore was sent " + getDateDifferenceMillisecond(match.startMatchTime));

        if (playerScore.score <= 0)
            playerScore.score = 0;

        socketClientManagerScript.manager.Socket.Emit("SendScore", JsonUtility.ToJson(playerScore));
        Time.timeScale = 0;

        Instantiate(finishConfettiEffectPrefab);
    }

    void ScoreResultListener(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("ScoreResultListener " + getDateDifferenceMillisecond(match.startMatchTime));

        Debug.Log("ScoreResultListener " + args[0].ToString());

        playersScore = JsonUtility.FromJson<PlayersScore>(args[0].ToString());

        if (args[1].ToString() == match.matchID)
            StartCoroutine(ScoreTableDisplay(playersScore));
    }

    IEnumerator ScoreTableDisplay(PlayersScore _playersScore)
    {
        Debug.Log("ScoreTableDisplay started " + _playersScore.playersScore.Count.ToString());
        playersScoreCanvas = Instantiate(playersScoreCanvasPrefab, playersScoreCanvasPrefab.transform.position, Quaternion.identity);

        Button closeButton = playersScoreCanvas.transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate () { screenManager.LoadScene(ScreenManager.Scene.RoomScreen); });

        for (int playerScoreOrder = _playersScore.playersScore.Count; playerScoreOrder > 0; playerScoreOrder--)
        {
            Debug.Log("ScoreTable object added");

            PlayerScore _playerScore = _playersScore.playersScore[playerScoreOrder - 1];

            GameObject playerScorePanel = Instantiate(playerScorePanelPrefab, playerScorePanelPrefab.transform.position, Quaternion.identity);

            Button playerButton = playerScorePanel.GetComponent<Button>();
            playerButton.onClick.AddListener(delegate ()
            {
                playerButtonOnClick(_playerScore.player);
            });

            PlayerScoreItemScript playerScoreItemScript = playerScorePanel.GetComponent<PlayerScoreItemScript>();
            playerScoreItemScript.playerScore = _playerScore;
            playerScoreItemScript.myRoomID = roomManagerScript.Room.roomID;
            playerScoreItemScript.playerScoreOrder = playerScoreOrder;

            playerScorePanel.transform.SetParent(playersScoreCanvas.transform.GetChild(0).GetChild(1).GetChild(3).GetChild(0).GetChild(0), false);
            playerScorePanel.transform.SetAsFirstSibling();

            if (_playerScore.player.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
                playerScoreComingFromServer = _playerScore;

            yield return new WaitForSecondsRealtime(playerScoreItemScript.scorePanelDisplayTime + playerScoreItemScript.scoreDisplayTime);

        }
        WinOrDefeatShow();
        HighScoreEffectEnable(playerScoreComingFromServer);
    }

    void HighScoreEffectEnable(PlayerScore _playerScore)
    {
        if (_playerScore != null)
        {
            if (PreviousPlayerStats.playerScores.playerGamesNoScore[match.game.gameNo].highScore < _playerScore.score)
            {
                Debug.Log("high score animation started");
                playersScoreCanvas.transform.GetChild(3).GetComponent<HighScoreEffectScript>().AnimationStart(_playerScore.score, match.game.gameName);
            }
        }
    }

    void WinOrDefeatShow()
    {
        if (playersScore.playersScore.Count > 0)
            switch (match.matchType)
            {
                case 1:
                    if (playersScore.playersScore[0].player.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
                    {
                        WinDefeatGUIActive(true, "You Are Win");
                    }
                    else
                    {
                        WinDefeatGUIActive(false, "You Are Defeated");
                    }
                    break;
                case 2:
                    if (playersScore.playersScore[0].player.playerRoomID == roomManagerScript.Room.roomID)
                    {
                        WinDefeatGUIActive(true, "Your Room Is Win");
                    }
                    else
                    {
                        WinDefeatGUIActive(false, "Your Room Is Defeated");
                    }
                    break;
                case 3:
                    if (playersScore.playersScore[0].player.playerRoomID == roomManagerScript.Room.roomID)
                    {
                        WinDefeatGUIActive(true, "Your Room Is Win");
                    }
                    else
                    {
                        WinDefeatGUIActive(false, "Your Room Is Defeated");
                    }
                    break;
            }
    }

    void WinDefeatGUIActive(bool areWin, string title)
    {
        Transform win = playersScoreCanvas.transform.GetChild(1);
        Transform defeated = playersScoreCanvas.transform.GetChild(2);
        if (areWin)
        {
            WinDefeatGUIDisplay(win, title);
        }
        else
        {
            WinDefeatGUIDisplay(defeated, title);
        }

    }

    void WinDefeatGUIDisplay(Transform winOrDefeatTransform, string title)
    {
        winOrDefeatTransform.gameObject.SetActive(true);
        winOrDefeatTransform.GetComponent<CanvasGroup>().DOFade(1, 2).SetUpdate(true);

        winOrDefeatTransform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = title;

        Button closeButton = winOrDefeatTransform.GetChild(1).GetChild(4).GetComponent<Button>();
        closeButton.onClick.AddListener(delegate () { winOrDefeatTransform.gameObject.SetActive(false); });

        PlayerScoreStatsGui(winOrDefeatTransform.GetChild(1).GetChild(3).GetChild(0));
    }

    void PlayerScoreStatsGui(Transform statsPanel)
    {
        PlayerScores newPlayerScores = socketClientManagerScript.MyPlayer.playerScores;
        PlayerScores previousPlayerScores = previousPlayerStats.playerScores;

        statsPanel.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = match.game.gameName + " Game Stats";

        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(0).GetChild(1).GetChild(0), newPlayerScores.playerGamesNoScore[match.game.gameNo].highScore, previousPlayerScores.playerGamesNoScore[match.game.gameNo].highScore, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(0).GetChild(1).GetChild(1), newPlayerScores.playerGamesNoScore[match.game.gameNo].avarageScore, previousPlayerScores.playerGamesNoScore[match.game.gameNo].avarageScore, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(0).GetChild(1).GetChild(2), newPlayerScores.playerGamesNoScore[match.game.gameNo].playedGamesNumber, previousPlayerScores.playerGamesNoScore[match.game.gameNo].playedGamesNumber, false);

        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(1).GetChild(1).GetChild(0), newPlayerScores.playerMainScore, previousPlayerScores.playerMainScore, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(1).GetChild(1).GetChild(1), newPlayerScores.playerWeeklyScore, previousPlayerScores.playerWeeklyScore, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(1).GetChild(1).GetChild(2), newPlayerScores.playerTotalMatchGames + newPlayerScores.playerTotalSoloGames + newPlayerScores.playerTotalTournamentGames, previousPlayerScores.playerTotalMatchGames + previousPlayerScores.playerTotalSoloGames + previousPlayerScores.playerTotalTournamentGames, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(1).GetChild(1).GetChild(3), newPlayerScores.playerMatchWins + newPlayerScores.playerSoloWins + newPlayerScores.playerTournamentWins, previousPlayerScores.playerMatchWins + previousPlayerScores.playerSoloWins + previousPlayerScores.playerTournamentWins, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(1).GetChild(1).GetChild(4),
            GetPercentageFloat(newPlayerScores.playerSoloWins + newPlayerScores.playerMatchWins + newPlayerScores.playerTournamentWins, newPlayerScores.playerTotalSoloGames + newPlayerScores.playerTotalMatchGames + newPlayerScores.playerTotalTournamentGames),
            GetPercentageFloat(previousPlayerScores.playerSoloWins + previousPlayerScores.playerMatchWins + previousPlayerScores.playerTournamentWins, previousPlayerScores.playerTotalSoloGames + previousPlayerScores.playerTotalMatchGames + previousPlayerScores.playerTotalTournamentGames), true);

        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(2).GetChild(1).GetChild(0), newPlayerScores.playerTotalMatchGames, previousPlayerScores.playerTotalMatchGames, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(2).GetChild(1).GetChild(1), newPlayerScores.playerMatchWins, previousPlayerScores.playerMatchWins, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(2).GetChild(1).GetChild(2),
            GetPercentageFloat(newPlayerScores.playerMatchWins, newPlayerScores.playerTotalMatchGames),
            GetPercentageFloat(previousPlayerScores.playerMatchWins, previousPlayerScores.playerTotalMatchGames), true);

        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(3).GetChild(1).GetChild(0), newPlayerScores.playerTotalSoloGames, previousPlayerScores.playerTotalSoloGames, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(3).GetChild(1).GetChild(1), newPlayerScores.playerSoloWins, previousPlayerScores.playerSoloWins, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(3).GetChild(1).GetChild(2),
            GetPercentageFloat(newPlayerScores.playerSoloWins, newPlayerScores.playerTotalSoloGames),
            GetPercentageFloat(previousPlayerScores.playerSoloWins, previousPlayerScores.playerTotalSoloGames), true);

        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(4).GetChild(1).GetChild(0), newPlayerScores.playerTotalTournamentGames, previousPlayerScores.playerTotalTournamentGames, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(4).GetChild(1).GetChild(1), newPlayerScores.playerTournamentWins, previousPlayerScores.playerTournamentWins, false);
        PlayerScoreStatsGuiUpdate(statsPanel.GetChild(4).GetChild(1).GetChild(2),
            GetPercentageFloat(newPlayerScores.playerTournamentWins, newPlayerScores.playerTotalTournamentGames),
            GetPercentageFloat(previousPlayerScores.playerTournamentWins, previousPlayerScores.playerTotalTournamentGames), true);
    }

    void PlayerScoreStatsGuiUpdate(Transform statPanel, float newScore, float previousScore, bool isPercent)
    {
        float scoreChange = newScore - previousScore;

        statPanel.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = Math.Round(scoreChange, 0).ToString();

        if (isPercent)
        {
            statPanel.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (scoreChange *= 100).ToString("0.00");
            statPanel.GetChild(3).GetComponent<TextMeshProUGUI>().text = newScore.ToString("0.00%");
        }
        else
        {
            statPanel.GetChild(3).GetComponent<TextMeshProUGUI>().text = Math.Round(newScore, 0).ToString();
        }

        if (scoreChange > 0)
        {
            statPanel.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        if (scoreChange < 0)
        {
            statPanel.GetChild(0).GetChild(2).gameObject.SetActive(true);
        }
    }

    void displayCountDownWaitForStart(float waitTime)
    {
        Debug.Log("displayCountDownWaitForStart was started");
        GameObject CountDownWaitForStart = Instantiate(countDownWaitForStartPrefab);
        CountDownWaitForStart.GetComponent<CountDownWaitForStartScript>().waitTime = waitTime;
    }

    void displayCountDownGameTime(float gameTime)
    {

        countDownGameTimeCanvas.GetComponent<CountDownGameTime>().SetGameTime(gameTime);
    }

    public void playerScoreAdd(int score)
    {
        playerScore.score += score;
    }
    void OnDestroy()
    {
        Debug.Log("GameScreenManager was destroyed");
        Screen.orientation = ScreenOrientation.AutoRotation;
        socketClientManagerScript.scoreResultEvent -= ScoreResultListener;
        Time.timeScale = 1;
    }

    void playerButtonOnClick(Player player)
    {
        Debug.Log("Click " + player.playerName);
        GameObject playerInfoPopup = Instantiate(playerInfoCanvasPrefab);
        playerInfoPopup.GetComponent<PlayerInfoScript>().player = player;
    }

    float GetPercentageFloat(int current, int maximum)
    {
        if (current == 0 || maximum == 0)
        {
            return 0;
        }
        return (float)current / (float)maximum;
    }

    float getDateDifferenceMillisecond(DateTime startDate)
    {
        TimeSpan timeSpan = DateTime.Now - startDate;
        return (float)timeSpan.TotalMilliseconds;
    }
}
