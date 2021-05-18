using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class SettingsToggleScript : MonoBehaviour
{
    public GameObject handler;
    public Image background;
    public Toggle toggle;

    public string setting_Data;

    Sprite onSprite;
    Sprite offSprite;
    void Start()
    {
        onSprite = Resources.Load<Sprite>("Buttons_Sprite/switch_on_bg");
        offSprite = Resources.Load<Sprite>("Buttons_Sprite/switch_off_bg");
        toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt(setting_Data));
    }

    public void SetToggleGUI()
    {
        Debug.Log("SetToggleGUI " + setting_Data + " " + toggle.isOn);

        PlayerPrefs.SetInt(setting_Data, Convert.ToInt32(toggle.isOn));
        PlayerPrefs.Save();

        if (toggle.isOn)
        {
            background.sprite = onSprite;
            handler.transform.DOLocalMoveX(43.1f, 0.1f);
        }
        else
        {
            background.sprite = offSprite;
            handler.transform.DOLocalMoveX(-43.1f, 0.1f);
        }
    }
}
