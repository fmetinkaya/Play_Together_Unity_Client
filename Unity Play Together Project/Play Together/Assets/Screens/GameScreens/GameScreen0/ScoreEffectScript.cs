using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class ScoreEffectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<CanvasGroup>().DOFade(0, 2).OnComplete(() => Destroy(transform.gameObject));
    }


}
