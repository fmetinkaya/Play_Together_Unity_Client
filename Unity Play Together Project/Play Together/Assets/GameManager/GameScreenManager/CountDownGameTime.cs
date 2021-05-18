using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownGameTime : MonoBehaviour
{
    private float gameTime;

    bool CountDownGameTimeEnable = false;

    void Start()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = camera;
        gameObject.transform.SetParent(camera.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        TimerCanvasUpdate();
    }

    void TimerCanvasUpdate()
    {
        if (CountDownGameTimeEnable)
        {
            gameTime -= Time.deltaTime;
            transform.GetChild(0).GetComponent<Slider>().value = gameTime;
        }
    }

    public void SetGameTime(float gameTime)
    {
        this.gameTime = gameTime;
        transform.GetChild(0).GetComponent<Slider>().maxValue = gameTime;
        transform.GetChild(0).GetComponent<Slider>().minValue = 0;

        CountDownGameTimeEnable = true;
    }

}
