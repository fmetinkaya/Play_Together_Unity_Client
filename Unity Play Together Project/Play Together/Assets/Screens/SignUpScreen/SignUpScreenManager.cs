using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BestHTTP;
using BestHTTP.SocketIO;

public class SignUpScreenManager : MonoBehaviour
{
    GameObject gameManager;
    SocketClientManager socketClientManagerScript;
    DialogueManager dialogueManagerScript;
    ScreenManager screenManager;
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField phoneInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordAgainInputField;

    public Button registerButton;
    public Button loginBackButton;
    public Button closeButton;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        socketClientManagerScript = gameManager.GetComponent<SocketClientManager>();
        dialogueManagerScript = gameManager.GetComponent<DialogueManager>();
        screenManager = gameManager.GetComponent<ScreenManager>();

        registerButton.onClick.AddListener(delegate () { singUp(nameInputField.text, emailInputField.text, phoneInputField.text, passwordInputField.text, passwordAgainInputField.text); });
        loginBackButton.onClick.AddListener(delegate () { screenManager.LoadScene(ScreenManager.Scene.LoginScreen); });
        closeButton.onClick.AddListener(delegate () { screenManager.LoadScene(ScreenManager.Scene.LoginScreen); });
    }

    void singUp(string userName, string userEmail, string userPhone, string password, string passwordAgain)
    {
        if (!userEmail.Contains("@"))
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Email isn't valid");
            return;
        }
        if (String.IsNullOrEmpty(userName))
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Nick name cannot be empty");
            return;
        }
        if (userPhone.Substring(0, 1) == "0")
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Phone number must not start with 0 for example 55499972xx");
            return;
        }
        if (userPhone.Length != 10)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Phone number must be 10 digits");
            return;
        }
        if (password.Length < 6)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Password cannot be less than 6 characters");
            return;
        }
        if (password != passwordAgain)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "Passwords are't the same");
            return;
        }
        Debug.Log("Sign Up Request");
        dialogueManagerScript.displayWaitingCanvas("singUp");
        socketClientManagerScript.manager.Socket.Emit("Sign Up", signUpCallBack, userName, userEmail, userPhone, password, false);
    }
    public void signUpCallBack(Socket socket, Packet originalPacket, params object[] args)
    {
        Debug.Log("Sign Up Request Callback");
        dialogueManagerScript.destroyWaitingCanvas();
        Debug.Log(args[0].ToString());
        if (Convert.ToInt32(args[0]) == 0)
        {
            Debug.Log("User Created");
            screenManager.LoadScene(ScreenManager.Scene.LoginScreen);
        }
        if (Convert.ToInt32(args[0]) == 1)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "There is already the email adress");
        }
        if (Convert.ToInt32(args[0]) == 2)
        {
            dialogueManagerScript.displayAlertCanvas("Operation Failed", "There is already the phone number");
        }
    }

}
