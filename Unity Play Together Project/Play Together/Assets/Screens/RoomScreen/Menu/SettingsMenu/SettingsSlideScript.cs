using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlideScript : MonoBehaviour
{
    public Slider settingSlider;
    public string SettingData;

    void Start()
    {

        settingSlider.value = PlayerPrefs.GetFloat(SettingData);
        settingSlider.onValueChanged.AddListener(delegate { UpdateSlider(); });

    }

    public void UpdateSlider()
    {
            PlayerPrefs.SetFloat(SettingData, settingSlider.value);
            PlayerPrefs.Save();
    }
}

