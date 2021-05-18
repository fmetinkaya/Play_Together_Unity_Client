using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuTapMenuScript : MonoBehaviour
{
    public List<GameObject> TapsGUI;
    public GameObject Chat;
    public GameObject Settings;

    void Start()
    {
        TapChange("Chat");
    }

    public void TapChange(string tap)
    {
        Debug.Log("Tap Menu was clicked to " + tap);
        foreach (GameObject tapGUI in TapsGUI)
        {
            if (tapGUI.name == tap)
            {
                tapGUI.transform.GetChild(0).gameObject.SetActive(true);
                tapGUI.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                if (tap == "Chat")
                    OpenTabChat();
                if (tap == "Settings")
                    OpenTabSettings();
            }
            else
            {
                tapGUI.transform.GetChild(0).gameObject.SetActive(false);
                tapGUI.GetComponent<TextMeshProUGUI>().color = new Color32(65, 105, 135, 255);
            }

        }
    }

    void OpenTabChat()
    {
        //Settings.SetActive(false);
        //Chat.SetActive(true);
        Settings.transform.localScale = Vector3.zero;
        Chat.transform.localScale = Vector3.one;
    }
    void OpenTabSettings()
    {
        //Chat.SetActive(false);
        //Settings.SetActive(true);
        Chat.transform.localScale = Vector3.zero;
        Settings.transform.localScale = Vector3.one;

    }
}
