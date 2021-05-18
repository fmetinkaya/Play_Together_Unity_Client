using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class confettiScript : MonoBehaviour
{
    GameObject gameManager;
    BackgroundMusicManager backgroundMusicManagerScript;
    Vector2 widthBoundaries;
    Vector2 heightBoundaries;
    public List<GameObject> confettiPrefab = new List<GameObject>();
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        backgroundMusicManagerScript = gameManager.GetComponent<BackgroundMusicManager>();
        backgroundMusicManagerScript.OnPauseBackgroundMusic(true);
        StartCoroutine(backgroundMusicResume(transform.GetComponent<AudioSource>().clip.length));

        transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
        transform.GetChild(0).GetComponent<Canvas>().planeDistance = 1;
        StartCoroutine(StartConfettiEffectCoroutine(true));
    }

    IEnumerator StartConfettiEffectCoroutine(bool isFirst)
    {
        if (isFirst)
        {
            Vector2 sizeDeltaCanvas = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta / 2;
            widthBoundaries = new Vector2(sizeDeltaCanvas.x, -sizeDeltaCanvas.x);
            heightBoundaries = new Vector2(sizeDeltaCanvas.y, -sizeDeltaCanvas.y);
        }

        for (int i = 0; i < 8; i++)
        {
            Vector3 position = new Vector3(Random.Range(widthBoundaries.x, widthBoundaries.y), Random.Range(heightBoundaries.x, heightBoundaries.y), 0);
            GameObject confetti = Instantiate(confettiPrefab[Random.Range(0, confettiPrefab.Count)]);
            confetti.transform.SetParent(transform.GetChild(0).transform, false);
            confetti.transform.SetAsFirstSibling();
            confetti.transform.localPosition = position;
            Destroy(confetti, 3);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(StartConfettiEffectCoroutine(false));
    }

    IEnumerator backgroundMusicResume(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        backgroundMusicManagerScript.OnPauseBackgroundMusic(false);
    }

}
