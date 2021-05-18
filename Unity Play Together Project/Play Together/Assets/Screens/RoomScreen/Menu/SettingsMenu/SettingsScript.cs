using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    public GameObject avatarsPanel;
    public GameObject selectedAvatar;
    public GameObject avatarListItemPrefab;
    public GameObject mainAvatar;
    Sprite[] avatarSprites;


    public TMP_InputField changeNameInputField;
    public TextMeshProUGUI changeNameMainField;
    public Button changeNameOkButton;


    public TMP_InputField changeRoomNameInputField;
    public TextMeshProUGUI changeRoomNameMainField;
    public Button changeRoomNameOkButton;


    public TextMeshProUGUI playerGlobalIDPanel;

    public Settings settings;

    void Start()
    {

        changeNameOkButton.onClick.AddListener(delegate ()
                          {
                              clickedchangeNameOkButton();
                          });
        changeRoomNameOkButton.onClick.AddListener(delegate ()
                           {
                               clickedchangeRoomNameOkButton();
                           });
    }
    public void SettingsGUIUpdate(Player myPlayer)
    {
        settings = PlayerToSettings(myPlayer);

        avatarSprites = new AvatarsScript().GetAvatarList();

        avatarListUpdate(settings.playerAvatar);

        changeNameInPutUpdate(settings.playerName);

        changeRoomNameInPutUpdate(settings.personalRoomName);

        playerGlobalIDPanel.text = settings.playerGlobalID;
    }
    public Settings PlayerToSettings(Player player)
    {
        return new Settings(player.playerName, player.playerAvatar, player.personalRoomName, player.playerGlobalID);
    }
    void changeNameInPutUpdate(string playerName)
    {
        settings.playerName = playerName;
        changeNameMainField.text = playerName;
        changeNameInputField.text = "";
        changeNameInputField.placeholder.GetComponent<TextMeshProUGUI>().text = playerName;

    }
    void clickedchangeNameOkButton()
    {
        if (changeNameInputField.text != "")
        {
            changeNameInPutUpdate(changeNameInputField.text);
        }
    }

    void changeRoomNameInPutUpdate(string playerRoomName)
    {
        settings.personalRoomName = playerRoomName;
        changeRoomNameMainField.text = playerRoomName;
        changeRoomNameInputField.text = "";
        changeRoomNameInputField.placeholder.GetComponent<TextMeshProUGUI>().text = playerRoomName;

    }

    void clickedchangeRoomNameOkButton()
    {
        if (changeRoomNameInputField.text != "")
        {
            changeRoomNameInPutUpdate(changeRoomNameInputField.text);
        }
    }
    void clickedOnAvatar(int i)
    {
        Debug.Log("clickedOnAvatar " + i);
        avatarListUpdate(i);
    }
    void avatarListUpdate(int playerAvatar)
    {
        settings.playerAvatar = playerAvatar;
        foreach (Transform child in avatarsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        mainAvatar.transform.GetComponent<Image>().sprite = avatarSprites[playerAvatar];
        selectedAvatar.transform.GetComponent<Image>().sprite = avatarSprites[playerAvatar];

        for (int i = 0; i < avatarSprites.Length; i++)
        {
            GameObject avatarListITem = Instantiate(avatarListItemPrefab);
            avatarListITem.name = i.ToString();

            avatarListITem.transform.SetParent(avatarsPanel.transform, false);

            avatarListITem.transform.GetComponent<Image>().sprite = avatarSprites[i];

            Button avatarButton = avatarListITem.transform.GetComponent<Button>();
            avatarButton.onClick.AddListener(delegate ()
                   {
                       int _i = i;
                       clickedOnAvatar(Convert.ToInt32(avatarListITem.name));
                   });

            if (playerAvatar == i)
            {
                avatarListITem.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                avatarListITem.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

}
