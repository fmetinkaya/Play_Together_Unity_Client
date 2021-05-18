using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class HelixScoreCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI textScore;
    public int score;

    void Start()
    {
        if (score == Game3Management.game3Management.ordinaryRemoveScore)
        {
            textScore.color = Color.red;
            textScore.text = score.ToString();
        }
        if (score == Game3Management.game3Management.ordinaryAddScore)
        {
            textScore.color = Color.white;
            textScore.text = "+" + score.ToString();
        }
        if (score == Game3Management.game3Management.extremeAddScore)
        {
            textScore.color = Color.green;
            textScore.text = "+" + score.ToString();
        }

        transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Random.Range(-150, 150), transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.y, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition3D.z);
        GetComponent<CanvasGroup>().DOFade(0, 1f);
        transform.GetChild(0).GetComponent<RectTransform>().DOAnchorPosY(0, 1f).OnComplete(() => { Destroy(gameObject); });
    }
}
