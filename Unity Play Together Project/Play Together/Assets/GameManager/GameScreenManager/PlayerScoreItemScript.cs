using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class PlayerScoreItemScript : MonoBehaviour
{

    TextMeshProUGUI playerScoreGUI;

    RectTransform panelRectTransform;


    int startScore = 0;
    public float scoreDisplayTime = 1f;
    public float scorePanelDisplayTime = 1f;
    float panelHeight = 152f;
    public PlayerScore playerScore;
    public string myRoomID;
    public int playerScoreOrder;
    void Start()
    {
        Debug.Log("score animation started");

        playerScoreGUI = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = playerScore.player.playerName;
        transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Image>().sprite = new AvatarsScript().GetAvatar(playerScore.player.playerAvatar);
        panelRectTransform = transform.GetComponent<RectTransform>();

        if (myRoomID == playerScore.player.playerRoomID)
        {
            Sprite PlayerAvatarbackground = Resources.Load<Sprite>("Ranking/ranking_profile_bg_blue");
            transform.GetChild(5).GetComponent<Image>().sprite = PlayerAvatarbackground;
        }
        else
        {
            Sprite PlayerAvatarbackground = Resources.Load<Sprite>("Ranking/ranking_profile_bg_red");
            transform.GetChild(5).GetComponent<Image>().sprite = PlayerAvatarbackground;
        }
        switch (playerScoreOrder)
        {
            case 1:
                transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 3:
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                transform.GetChild(3).gameObject.SetActive(true);
                transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = playerScoreOrder.ToString();
                break;
        }


        Sequence mySequence = DOTween.Sequence().SetUpdate(true);
        mySequence.Append(panelRectTransform.DOSizeDelta(new Vector2(0, panelHeight), scoreDisplayTime, false));
        mySequence.Append(DOTween.To(GetScoreFromZero, SetScoreToGUI, playerScore.score, scorePanelDisplayTime));
        mySequence.OnComplete(() =>
        {
            Debug.Log("score animation was finished");
        });


    }

    int GetScoreFromZero()
    {
        return startScore;
    }

    void SetScoreToGUI(int newValue)
    {
        startScore = newValue;
        playerScoreGUI.text = startScore.ToString();
    }


}
