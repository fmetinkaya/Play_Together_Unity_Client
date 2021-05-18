using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("RememberMe"))
        {
            PlayerPrefs.SetInt("RememberMe", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Setting_PushAlarm"))
        {
            PlayerPrefs.SetInt("Setting_PushAlarm", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Setting_SoundFx"))
        {
            PlayerPrefs.SetFloat("Setting_SoundFx", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Setting_Vibration"))
        {
            PlayerPrefs.SetInt("Setting_Vibration", 1);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("Setting_Music"))
        {
            PlayerPrefs.SetFloat("Setting_Music", 0.3f);
            PlayerPrefs.Save();
        }
    }

}
