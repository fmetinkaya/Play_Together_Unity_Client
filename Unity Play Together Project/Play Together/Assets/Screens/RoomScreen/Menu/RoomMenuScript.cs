using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class RoomMenuScript : MonoBehaviour
{
    bool isOpen = false;
    public RectTransform rt;

    public SettingsScript settingsScript;

    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;

    public GameObject dimmerBackground;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        dialogueManagerScript = gameManager.GetComponent<DialogueManager>();
    }
    public void MenuOpenClose()
    {
        if (isOpen)
        {
            isOpen = false;
            dimmerBackground.SetActive(false);
            rt.DOLocalMoveX(0, 1);

            SendSettings(settingsScript.settings);
        }
        else
        {
            isOpen = true;
            dimmerBackground.SetActive(true);
            rt.DOLocalMoveX(850, 1);

            UpdateSettingsGUI(socketClientManagerScript.MyPlayer);
        }
    }

    void SendSettings(Settings settings)
    {
        Debug.Log("SendSettings ");
        if (!settings.Equals(settingsScript.PlayerToSettings(socketClientManagerScript.MyPlayer)))
        {
            dialogueManagerScript.displayWaitingCanvas("SendSettings");
            socketClientManagerScript.manager.Socket.Emit("SendSettings", JsonUtility.ToJson(settings));
        }
    }
    void UpdateSettingsGUI(Player myPlayer)
    {
        settingsScript.SettingsGUIUpdate(myPlayer);
    }
}
