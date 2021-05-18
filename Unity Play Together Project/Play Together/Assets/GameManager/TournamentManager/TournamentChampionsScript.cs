using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TournamentChampionsScript : MonoBehaviour
{
    public enum State
    {
        Tournaments,
        Rooms,
        Players
    }

    public static int TOURNAMENT_LIMIT = 10;
    public static int ROOM_LIMIT = 100;

    public State state = State.Tournaments;

    public GameObject tournamentsScrollRect;
    public GameObject roomsScrollRect;
    public GameObject playersScrollRect;


    public GameObject TournamentChampionListItemPrefab;
    public GameObject TCPlayerListItemPrefab;
    public GameObject TCRoomListItemPrefab;


    public Button backButton;
    public Button closeButton;

    public TournamentChampions tournamentChampions;
    void Start()
    {
        Debug.Log("Tournament Champions Script was started" + JsonUtility.ToJson(tournamentChampions));

        closeButton.onClick.AddListener(delegate ()
            {
                Destroy(gameObject);
            });
        backButton.onClick.AddListener(delegate ()
            {
                clickOnBackButton();
            });

        StartTournaments();
    }

    void StartTournaments()
    {

        state = State.Tournaments;
        backButton.transform.gameObject.SetActive(false);
        tournamentsScrollRect.SetActive(true);
        roomsScrollRect.SetActive(false);

        clearContent(tournamentsScrollRect.transform.GetChild(0));

        for (int i = 0; (i < tournamentChampions.tournamentChampions.Count && i < TOURNAMENT_LIMIT); i++)
        {
            GameObject TournamentChampionListItem = Instantiate(TournamentChampionListItemPrefab);
            TournamentChampionListItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToDateTime(tournamentChampions.tournamentChampions[i].tournamentDate).ToString("MM/dd/yyyy HH:mm");
            TournamentChampionListItem.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = tournamentChampions.tournamentChampions[i].TCRooms.Count.ToString();
            TournamentChampionListItem.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = tournamentChampions.tournamentChampions[i].playedGames.ToString();

            int Tcount = i;
            TournamentChampionListItem.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                StartRooms(Tcount);
            });


            TournamentChampionListItem.transform.SetParent(tournamentsScrollRect.transform.GetChild(0), false);
        }
    }

    void StartRooms(int Tcount)
    {
        state = State.Rooms;
        backButton.transform.gameObject.SetActive(true);
        tournamentsScrollRect.SetActive(false);
        roomsScrollRect.SetActive(true);
        playersScrollRect.SetActive(false);

        clearContent(roomsScrollRect.transform.GetChild(0));

        List<TCRoom> _TCRooms = tournamentChampions.tournamentChampions[Tcount].TCRooms;

        for (int i = 0; (i < _TCRooms.Count && i < ROOM_LIMIT); i++)
        {
            GameObject TCRoomListItem = Instantiate(TCRoomListItemPrefab);

            switch (i + 1)
            {
                case 1:
                    TCRoomListItem.transform.GetChild(0).gameObject.SetActive(true);
                    TCRoomListItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Ranking/ranking_medal_gold");
                    break;
                case 2:
                    TCRoomListItem.transform.GetChild(0).gameObject.SetActive(true);
                    TCRoomListItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Ranking/ranking_medal_silver");
                    break;
                case 3:
                    TCRoomListItem.transform.GetChild(0).gameObject.SetActive(true);
                    TCRoomListItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Ranking/ranking_medal_bronze");
                    break;
                default:
                    TCRoomListItem.transform.GetChild(1).gameObject.SetActive(true);
                    TCRoomListItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
                    break;
            }
            TCRoomListItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = _TCRooms[i].roomName;
            TCRoomListItem.transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = _TCRooms[i].TCPlayers.Count.ToString();
            TCRoomListItem.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = _TCRooms[i].wonGames + "/" + tournamentChampions.tournamentChampions[Tcount].playedGames;

            int Rcount = i;
            TCRoomListItem.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                StartPlayers(Rcount, Tcount);
            });


            TCRoomListItem.transform.SetParent(roomsScrollRect.transform.GetChild(0), false);
        }

    }

    void StartPlayers(int Rcount, int Tcount)
    {
        state = State.Players;
        tournamentsScrollRect.SetActive(false);
        roomsScrollRect.SetActive(false);
        playersScrollRect.SetActive(true);

        clearContent(playersScrollRect.transform.GetChild(0));

        List<TCPlayer> _TCPlayers = tournamentChampions.tournamentChampions[Tcount].TCRooms[Rcount].TCPlayers;
        for (int i = 0; (i < _TCPlayers.Count); i++)
        {
            GameObject TCPlayerListItem = Instantiate(TCPlayerListItemPrefab);

            TCPlayerListItem.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = new AvatarsScript().GetAvatar(_TCPlayers[i].playerAvatar);

            TCPlayerListItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _TCPlayers[i].playerName;

            TCPlayerListItem.transform.SetParent(playersScrollRect.transform.GetChild(0), false);
        }

    }

    void clickOnBackButton()
    {
        switch (state)
        {
            case State.Rooms:
                state = State.Tournaments;
                backButton.transform.gameObject.SetActive(false);
                tournamentsScrollRect.SetActive(true);
                roomsScrollRect.SetActive(false);
                break;
            case State.Players:
                state = State.Rooms;
                roomsScrollRect.SetActive(true);
                playersScrollRect.SetActive(false);
                break;
        }
    }

    void clearContent(Transform content)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

}
