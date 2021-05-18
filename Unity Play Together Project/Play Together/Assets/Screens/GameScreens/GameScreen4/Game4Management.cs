using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class Game4Management : MonoBehaviour
{

    GameObject gameScreenManager;
    GameScreenManager gameScreenManagerScript;
    public GameObject ScoreTextGUIPrefab;
    public GameObject ScoreCanvas;
    public static Game4Management game4Management;
    public GameObject HoleParent;
    HoleParentScript holeParentScript;

    Vector3 holeCurrentScale = new Vector3();
    Sequence mySequence;

    public GameObject[] spawnGameObjects;

    RandomManager randomManager;
    void Start()
    {
        gameScreenManager = GameObject.FindGameObjectWithTag("GameScreenManager");
        gameScreenManagerScript = gameScreenManager.GetComponent<GameScreenManager>();
        randomManager = gameScreenManagerScript.GetNewRandomManager();
        HoleParent.transform.position = spawnGameObjects[Mathf.FloorToInt(randomManager.RandomGenerate(0, 7))].transform.position;

        mySequence = DOTween.Sequence();
        holeCurrentScale = HoleParent.transform.localScale;
        Physics.gravity = new Vector3(0, -40, 0);
        game4Management = this;
        holeParentScript = HoleParent.GetComponent<HoleParentScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addScore(float volume)
    {
        //Debug.Log(volume);
        float increaseSizeAmount = volume / ((Mathf.Pow(HoleParent.transform.localScale.x, 2) * 7));

        increaseSizeHole(increaseSizeAmount);
        int score = Mathf.RoundToInt(increaseSizeAmount * 1000);

        GameObject ScoreTextGUI = Instantiate(ScoreTextGUIPrefab);
        ScoreTextGUI.GetComponent<TextMeshProUGUI>().text = "+" + score;
        ScoreTextGUI.transform.SetParent(ScoreCanvas.transform, false);

        ScoreTextGUI.GetComponent<TextMeshProUGUI>().DOFade(0.5f, 1);
        ScoreTextGUI.GetComponent<RectTransform>().DOLocalMoveY(200, 1).OnComplete(() => { Destroy(ScoreTextGUI); });

        gameScreenManagerScript.playerScoreAdd(score);
    }

    public void increaseSizeHole(float addSize)
    {
        holeCurrentScale = holeCurrentScale + new Vector3(addSize, 0, addSize);

        mySequence.Kill();
        mySequence.Append(HoleParent.GetComponent<Transform>().DOScale(holeCurrentScale, 0.5f));

        if (HoleParent.transform.localScale.x > 35)
        {
            HoleParent.transform.localScale = new Vector3(30, HoleParent.transform.localScale.y, 30);
        }
    }

}
