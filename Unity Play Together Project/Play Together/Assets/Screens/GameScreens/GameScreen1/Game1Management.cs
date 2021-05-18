using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Game1Management : MonoBehaviour
{

    public BirdScoreCanvas birdScoreScript;
    public static Game1Management game1Management;
    public GameScreenManager gameScreenManagerScript;

    int increasingScoreIncreasingAmount = 5;
    int increasingScore = 0;
    int increasingAmount = 10;

    public float gameMoveSpeed;
    public float birdUpForce;
    void Awake()
    {
        game1Management = this;
    }
    void Start()
    {
        birdScoreScript.ShowScore("GO");
    }

    public void RestartGame()
    {
        GetComponent<BirdObstaclesManager>().restartPositionObstacle();

        birdScoreScript.ShowScore("GO");
        birdScoreScript.ScreenFlash(1.5f);
        Camera.main.DOShakePosition(1f, 0.1f).OnComplete(() =>
                {
                    Camera.main.transform.position = new Vector3(0, 0, -10);
                });



        increasingScore = 0;
    }
    public void AddScore()
    {
        int amount = increasingAmount + increasingScore;
        increasingScore = increasingScore + increasingScoreIncreasingAmount;
        birdScoreScript.ShowScore("+" + amount.ToString());

        gameScreenManagerScript.playerScoreAdd(amount);
    }


}
