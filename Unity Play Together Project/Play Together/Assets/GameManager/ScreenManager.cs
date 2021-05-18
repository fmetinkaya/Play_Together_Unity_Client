using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public Action<string> loadSceneEvent;
    public enum Scene
    {
        InitialScreen,
        LoginScreen,
        RoomScreen,
        SignUpScreen
    }


    public void LoadScene(Scene scene)
    {
        loadSceneEvent(scene.ToString());
        SceneManager.LoadScene(scene.ToString());
    }

    public void LoadGameScene(int gameNo)
    {
        loadSceneEvent("GameScreen" + gameNo);
        SceneManager.LoadScene("GameScreen" + gameNo);
    }

}
