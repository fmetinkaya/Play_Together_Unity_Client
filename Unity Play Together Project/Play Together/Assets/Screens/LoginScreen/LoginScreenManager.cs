using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BestHTTP;
using BestHTTP.SocketIO;



public class LoginScreenManager : MonoBehaviour
{
    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;
    ScreenManager screenManager;

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public Button signUpButton;
    public Toggle rememberMEToggle;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        dialogueManagerScript = gameManager.GetComponent<DialogueManager>();
        screenManager = gameManager.GetComponent<ScreenManager>();

        emailInputField.text = PlayerPrefs.GetString("RememberUserEmail");
        passwordInputField.text = PlayerPrefs.GetString("RememberUserPassword");

        loginButton.onClick.AddListener(delegate ()
        {
            if (PlayerPrefs.GetInt("RememberMe") == 1)
            {
                PlayerPrefs.SetString("RememberUserEmail", emailInputField.text);
                PlayerPrefs.SetString("RememberUserPassword", passwordInputField.text);
                PlayerPrefs.Save();
            }
            login(emailInputField.text, passwordInputField.text);
        });

        signUpButton.onClick.AddListener(delegate () { screenManager.LoadScene(ScreenManager.Scene.SignUpScreen); });

        rememberMEToggle.isOn = (Convert.ToBoolean(PlayerPrefs.GetInt("RememberMe")));
        rememberMEToggle.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                PlayerPrefs.SetInt("RememberMe", 1);
            }
            else
            {
                PlayerPrefs.SetInt("RememberMe", 0);
                PlayerPrefs.DeleteKey("RememberUserEmail");
                PlayerPrefs.DeleteKey("RememberUserPassword");
            }
            PlayerPrefs.Save();
        }
       );
    }
    public void login(String userEmail, String password)
    {
        Debug.Log("Login Request");
        dialogueManagerScript.displayWaitingCanvas("login");
        socketClientManagerScript.manager.Socket.Emit("Login", loginCallBack, userEmail, password, "");
    }
    public void loginCallBack(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("Login Request Callback " + args[0].ToString());
        dialogueManagerScript.destroyWaitingCanvas();
        if (Convert.ToInt32(args[0]) == 0)
        {
            Debug.Log("User Logged");

            screenManager.LoadScene(ScreenManager.Scene.RoomScreen);
        }
        if (Convert.ToInt32(args[0]) == 1)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Password Is Wrong");
        }
        if (Convert.ToInt32(args[0]) == 2)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "User Not Registered");
        }
        if (Convert.ToInt32(args[0]) == 3)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "This User Is Already Online");
        }
    }

}
