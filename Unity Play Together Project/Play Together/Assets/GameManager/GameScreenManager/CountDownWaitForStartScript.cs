using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CountDownWaitForStartScript : MonoBehaviour
{
    public Image timerImageGUI;
    public TextMeshProUGUI timerTextGUI;

    public float waitTime;
    DateTime startDate;
    float startTime;
    void Start()
    {
        startDate = DateTime.Now;
        startDate = startDate.AddMilliseconds(waitTime);
        startTime = getDateDifferenceMillisecond(startDate);
    }
    void Update()
    {
        updateTimerGUI();
    }

    void updateTimerGUI()
    {
        if (getDateDifferenceMillisecond(startDate) < 0)
        {
            Destroy(gameObject);
        }

        float fillAmount = getDateDifferenceMillisecond(startDate) / startTime;
        timerImageGUI.fillAmount = fillAmount;

        timerTextGUI.text = Math.Round(getDateDifferenceMillisecond(startDate) / 1000, 0).ToString();
    }
    float getDateDifferenceMillisecond(DateTime startDate)
    {
        TimeSpan timeSpan = startDate - DateTime.Now;
        return (float)timeSpan.TotalMilliseconds;
    }
}
