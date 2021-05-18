using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BestHTTP.SocketIO;
using System;


public class ChatScript : MonoBehaviour
{
    public Button chatSendMessageButton;
    public TMP_InputField chatMessageInputField;
    public GameObject chatMessageAnyoneGUIPrefab;
    public GameObject chatMessageMeGUIPrefab;
    public GameObject chatMessageContent;

    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    RoomManager roomManagerScript;
    ChatManager chatManagerScript;

    public ScrollRect roomScrollRect;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        roomManagerScript = gameManager.GetComponent<RoomManager>();
        chatManagerScript = gameManager.GetComponent<ChatManager>();

        roomManagerScript.roomChangedEvent += roomChangedListener;
        socketClientManagerScript.receiveMessageEvent += ReceiveMessageListener;

        Button playerButton = chatSendMessageButton.GetComponent<Button>();
        playerButton.onClick.AddListener(delegate ()
       {
           SendMessage();
       });

        addMessageGUI(chatManagerScript.chatMessages);
    }

    void SendMessage()
    {
        Debug.Log("SendMessage " + chatMessageInputField.text);
        if (chatMessageInputField.text != "")
        {
            ChatMessage chatMessage = new ChatMessage(chatMessageInputField.text, socketClientManagerScript.MyPlayer.playerName, socketClientManagerScript.MyPlayer.playerAvatar, socketClientManagerScript.MyPlayer.playerGlobalID, "", roomManagerScript.Room.roomID);
            socketClientManagerScript.manager.Socket.Emit("SendMessage", JsonUtility.ToJson(chatMessage));
            chatMessageInputField.text = "";
        }

    }
    void ReceiveMessageListener(Socket s, Packet p, object[] a)
    {
        Debug.Log("ReceiveMessageListener " + a[0].ToString());
        ChatMessage chatMessage = JsonUtility.FromJson<ChatMessage>(a[0].ToString());
        List<ChatMessage> chatMessages = new List<ChatMessage>();
        chatMessages.Add(chatMessage);
        addMessageGUI(chatMessages);
    }
    void roomChangedListener(string previousRoomID, string newRoomID)
    {
        Debug.Log("roomChangedListener previous roomID" + previousRoomID + " new roomID " + newRoomID);
        clearMessageGUI();
    }

    void clearMessageGUI()
    {
        foreach (Transform child in chatMessageContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    void addMessageGUI(List<ChatMessage> chatMessages)
    {
        Debug.Log("addMessageGUI " + chatMessages);
        foreach (ChatMessage chatMessage in chatMessages)
        {
            GameObject chatMessageGUI;
            if (chatMessage.playerGlobalID == socketClientManagerScript.MyPlayer.playerGlobalID)
            {
                chatMessageGUI = Instantiate(chatMessageMeGUIPrefab);
            }
            else
            {
                chatMessageGUI = Instantiate(chatMessageAnyoneGUIPrefab);
            }
            chatMessageGUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = new AvatarsScript().GetAvatar(chatMessage.playerAvatar);
            chatMessageGUI.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = chatMessage.playerName;
            chatMessageGUI.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = chatMessageDateConvertation(chatMessage.date);

            RectTransform messageGUIRectTransform = chatMessageGUI.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
            messageGUIRectTransform.GetComponent<TextMeshProUGUI>().text = chatMessage.message;

            chatMessageGUI.GetComponent<RectTransform>().sizeDelta = new Vector2(chatMessageGUI.GetComponent<RectTransform>().sizeDelta.x, 160 + CharacterToHeight(messageGUIRectTransform.GetComponent<TextMeshProUGUI>().text.Length));

            chatMessageGUI.transform.SetParent(chatMessageContent.transform, false);

            StartCoroutine(ForceScrollDown(roomScrollRect));

        }
    }

    string chatMessageDateConvertation(string date)
    {
        DateTime dateTime = Convert.ToDateTime(date);
        return (dateTime.ToString("HH:mm"));
    }
    IEnumerator ForceScrollDown(ScrollRect scrollRect)
    {
        Debug.Log("ForceScrollDown ");
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    float CharacterToHeight(int messageCharacterCount)
    {
        int mod = messageCharacterCount % 28;
        int div = messageCharacterCount / 28;

        messageCharacterCount = div;
        if (mod > 0)
        {
            messageCharacterCount = messageCharacterCount + 1;
        }

        float speechBubbleSize = (messageCharacterCount - 2) * 43.44f;
        if (speechBubbleSize < 0)
        {
            speechBubbleSize = 0;
        }

        return speechBubbleSize;
    }

    void OnDestroy()
    {
        print("ChatScript was destroyed");
        roomManagerScript.roomChangedEvent -= roomChangedListener;
        socketClientManagerScript.receiveMessageEvent -= ReceiveMessageListener;
    }
}
