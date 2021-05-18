using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BirdScoreCanvas : MonoBehaviour
{
    public void ScreenFlash(float duration)
    {
        DOTween.Kill(transform.GetChild(1).GetComponent<CanvasGroup>());

        transform.GetChild(1).GetComponent<CanvasGroup>().alpha = 1;
        transform.GetChild(1).GetComponent<CanvasGroup>().DOFade(0, duration);
    }
    public void ShowScore(string scoreText)
    {
        DOTween.Kill(transform.GetChild(0).GetComponent<CanvasGroup>());

        transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
        transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 1.5f);
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = scoreText;
    }
}
