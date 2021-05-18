using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game3Management : MonoBehaviour
{
    GameObject gameScreenManager;
    GameScreenManager gameScreenManagerScript;
    public int ordinaryAddScore = 10;
    public int ordinaryRemoveScore = -100;
    public int extremeAddScore = 50;
    public GameObject scoreCanvasPrefab;
    public static Game3Management game3Management;
    public GameObject realCyclinder;
    List<GameObject> helixPool = new List<GameObject>();
    public GameObject helixPrefab;
    float distanceBetweenHelix = 0.15f;
    int nextHelixNumber = 0;

    int helixPoolSize = 20;
    int nextHelixAtPool = 0;

    RandomManager randomManager;
    void Start()
    {
        gameScreenManager = GameObject.FindGameObjectWithTag("GameScreenManager");
        gameScreenManagerScript = gameScreenManager.GetComponent<GameScreenManager>();

        randomManager = gameScreenManagerScript.GetNewRandomManager();

        game3Management = this;

        HelixFirstPoolSpawn();
    }

    public void SpawnHelixes(GameObject helixGameObject)
    {
        HelixPoolSpawn(helixGameObject);
    }

    void HelixPoolSpawn(GameObject helixGameobject)
    {
        List<int> notAssignedHelix = new List<int>();
        for (int i = 0; i < 12; i++)
        {
            notAssignedHelix.Add(i);
        }

        int selectedRealHelix = notAssignedHelix[(int)randomManager.RandomGenerate(0, notAssignedHelix.Count)];
        notAssignedHelix.Remove(selectedRealHelix);
        int selectedHideHelix = notAssignedHelix[(int)randomManager.RandomGenerate(0, notAssignedHelix.Count)];
        notAssignedHelix.Remove(selectedRealHelix);

        for (int k = 0; k < 12; k++)
        {
            if (k == selectedRealHelix)
            {
                helixGameobject.transform.GetChild(k).GetComponent<HelixPieceScript>().SetState(HelixPieceScript.HelixState.real);
            }
            else if (k == selectedHideHelix)
            {
                helixGameobject.transform.GetChild(k).GetComponent<HelixPieceScript>().SetState(HelixPieceScript.HelixState.hide);
            }
            else
            {
                int randomSelectNumber = (int)randomManager.RandomGenerate(0, 11);
                if (randomSelectNumber < 5)
                {
                    helixGameobject.transform.GetChild(k).GetComponent<HelixPieceScript>().SetState(HelixPieceScript.HelixState.real);
                }
                if (5 <= randomSelectNumber && randomSelectNumber <= 9)
                {
                    helixGameobject.transform.GetChild(k).GetComponent<HelixPieceScript>().SetState(HelixPieceScript.HelixState.hide);
                }
                if (randomSelectNumber == 10)
                {
                    helixGameobject.transform.GetChild(k).GetComponent<HelixPieceScript>().SetState(HelixPieceScript.HelixState.obstacle);
                }
            }
        }

        helixGameobject.transform.localPosition = new Vector3(helixGameobject.transform.localPosition.x, -nextHelixNumber * distanceBetweenHelix, helixGameobject.transform.localPosition.z);

        nextHelixNumber += 1;
    }
    void HelixFirstPoolSpawn()
    {
        for (int i = 0; i < helixPoolSize; i++)
        {
            GameObject helixGameobject = Instantiate(helixPrefab, new Vector3(0, -nextHelixNumber * distanceBetweenHelix, 0), Quaternion.identity);
            helixPool.Add(helixGameobject);
            helixGameobject.transform.SetParent(realCyclinder.transform, false);
            HelixPoolSpawn(helixGameobject);
        }
    }
    public void scoreDisplay(int score)
    {
        GameObject scoreCanvas = Instantiate(scoreCanvasPrefab);
        scoreCanvas.GetComponent<HelixScoreCanvasScript>().score = score;

        gameScreenManagerScript.playerScoreAdd(score);
    }
}
