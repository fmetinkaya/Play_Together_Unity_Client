using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class HighScoreEffectScript : MonoBehaviour
{
    public void AnimationStart(int scoreNew, string gameName)
    {
        gameObject.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Highest Score In " + gameName;
        gameObject.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreNew.ToString();

        gameObject.SetActive(true);
        gameObject.GetComponent<RectTransform>().DOShakeAnchorPos(4, 100f, 10, 90f).SetUpdate(true);
        gameObject.GetComponent<CanvasGroup>().DOFade(1, 3).SetUpdate(true).OnComplete(() => { gameObject.GetComponent<CanvasGroup>().DOFade(0, 3).SetUpdate(true).OnComplete(() => { gameObject.SetActive(false); }); });

    }
}
