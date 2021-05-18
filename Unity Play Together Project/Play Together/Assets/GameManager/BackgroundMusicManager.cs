using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> backgroundMusicClips = new List<AudioClip>();
    ScreenManager screenManager;

    bool isOnGameScreen = false;

    void Start()
    {
        screenManager = gameObject.GetComponent<ScreenManager>();
        screenManager.loadSceneEvent += loadScreenListener;
    }
    void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Setting_Music");

        nextBackgroundMusic();
    }

    void nextBackgroundMusic()
    {
        if (!audioSource.isPlaying && !isOnGameScreen)
        {
            audioSource.clip = backgroundMusicClips[UnityEngine.Random.Range(0, backgroundMusicClips.Count)];
            audioSource.Play();
        }
    }

    void loadScreenListener(string screenName)
    {
        if (screenName.Contains("GameScreen2"))
        {
            OnPauseBackgroundMusic(true);
        }
        else
        {
            OnPauseBackgroundMusic(false);
        }
    }

    public void OnPauseBackgroundMusic(bool isPause)
    {

        if (isPause)
        {
            audioSource.Pause();
            isOnGameScreen = true;
        }
        else
        {
            isOnGameScreen = false;
            audioSource.UnPause();
        }
    }
}
