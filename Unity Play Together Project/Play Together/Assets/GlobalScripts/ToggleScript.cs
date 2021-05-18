using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ToggleScript : MonoBehaviour
{
    public GameObject handler;
    public Image background;
    public Toggle toggle;

    Sprite onSprite;
    Sprite offSprite;
    void Start()
    {
        onSprite = Resources.Load<Sprite>("Buttons_Sprite/switch_on_bg");
        offSprite = Resources.Load<Sprite>("Buttons_Sprite/switch_off_bg");
        StartToggleGUI();
    }

    public void SetToggleGUI()
    {
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
    public void StartToggleGUI()
    {
        if (toggle.isOn)
        {
            background.sprite = onSprite;
            handler.transform.DOLocalMoveX(43.1f, 0f);
        }
        else
        {
            background.sprite = offSprite;
            handler.transform.DOLocalMoveX(-43.1f, 0f);
        }
    }
}
