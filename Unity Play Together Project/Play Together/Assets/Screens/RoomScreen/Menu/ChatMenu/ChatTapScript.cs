using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatTapScript : MonoBehaviour
{
    public List<GameObject> TapsGUI;
    public GameObject Room;
    public GameObject Family;

    void Start()
    {
        TapChange("Room");
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
                if (tap == "Room")
                    OpenTabRoom();
                if (tap == "Family")
                    OpenTabFamily();
            }
            else
            {
                tapGUI.transform.GetChild(0).gameObject.SetActive(false);
                tapGUI.GetComponent<TextMeshProUGUI>().color = new Color32(65, 105, 135, 255);
            }

        }
    }

    void OpenTabRoom()
    {
        Family.transform.localScale = Vector3.zero;
        Room.transform.localScale = Vector3.one;
    }
    void OpenTabFamily()
    {
        Room.transform.localScale = Vector3.zero;
        Family.transform.localScale = Vector3.one;
    }
}
