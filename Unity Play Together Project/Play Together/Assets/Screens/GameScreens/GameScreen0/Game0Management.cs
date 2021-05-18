using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Game0Management : MonoBehaviour
{
    public GameObject scorePrefab;
    GameObject gameScreenManager;
    GameScreenManager gameScreenManagerScript;
    public GameObject targetPrefab;

    RandomManager randomManager;

    void Start()
    {
        gameScreenManager = GameObject.FindGameObjectWithTag("GameScreenManager");
        gameScreenManagerScript = gameScreenManager.GetComponent<GameScreenManager>();

        randomManager = gameScreenManagerScript.GetNewRandomManager();

        StartCoroutine("CreateTarget");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CreateTarget()
    {
        while (true)
        {
            GameObject target = Instantiate(targetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
            target.transform.SetPositionAndRotation(new Vector3(randomManager.RandomGenerate(0, 8.5f - (sr.bounds.size.x / 2f)), -5 + (sr.bounds.size.y / 2f), 0), Quaternion.identity);
            yield return new WaitForSeconds(3);

        }

    }
    public void ScoreEffectCreate(Vector3 screenPosition, int score, Color backgroundColor, Color textColor)
    {
        GameObject scoreEffect = Instantiate(scorePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        scoreEffect.transform.SetPositionAndRotation(screenPosition, Quaternion.identity);
        scoreEffect.transform.GetChild(0).GetComponent<Image>().color = backgroundColor;
        scoreEffect.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
        scoreEffect.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = textColor;

        gameScreenManagerScript.playerScoreAdd(score);
    }

}
